using DoomBot.Server.MongoDB;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using IdGen;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace DoomBot.Server.Modules
{
    public class ModuleDataWrapper<T>: IDisposable
    {
        protected ModuleDataWrapper()
        {
            //Nothing here xd
        }
        
        protected static Action<ModuleDataWrapper<T>> Callback;

        public BsonValue ID { get; protected set; }
        
        public T Data { get; protected set; }
        
        public void Dispose()
        {
            Callback(this);
        }
    }
    
    public class DoomModuleBase<T>
    {
        private class ModuleDataPoolComparer : IEqualityComparer<ModuleDataWrapper<T>>
        {
            public bool Equals(ModuleDataWrapper<T> X, ModuleDataWrapper<T> Y)
            {
                return X.ID == Y.ID;
            }

            public int GetHashCode(ModuleDataWrapper<T> Obj)
            {
                return Obj.ID.GetHashCode();
            }
        }
        
        private class ModuleDataWrapperInternal: ModuleDataWrapper<T>
        {
            public static void SetCallback(Action<ModuleDataWrapper<T>> Callback)
            {
                ModuleDataWrapper<T>.Callback = Callback;
            }

            public void SetID(BsonValue ID)
            {
                this.ID = ID;
            }
            
            public void SetData(T Data)
            {
                this.Data = Data;
            }
        }
        
        private readonly string CollectionName;
        
        private readonly MDB DB;
        
        private readonly HashSet<ModuleDataWrapper<T>> Pool;

        private readonly HashSet<ModuleDataWrapper<T>> UpdateQueue;

        private readonly Queue<ModuleDataWrapperInternal> InternalPool;

        protected DoomModuleBase(string CollectionName, MDB DB)
        {
            ModuleDataWrapperInternal.SetCallback(WrapperCallback);
            
            this.CollectionName = CollectionName;
            
            this.DB = DB;

            var Comparer = new ModuleDataPoolComparer();
            
            Pool = new HashSet<ModuleDataWrapper<T>>(Comparer);
            
            UpdateQueue = new HashSet<ModuleDataWrapper<T>>(Comparer);

            InternalPool = new Queue<ModuleDataWrapperInternal>();

            Task.Run(Loop);
        }

        private async Task Loop()
        {
            var TaskList = new List<Task>();
            
            while (true)
            {
                //Update Data

                foreach (var Data in UpdateQueue)
                {
                    Task.Run(async () => await UpdateData(Data));
                }
                
                await Task.Delay(10000);
            }
        }
        
        private void WrapperCallback(ModuleDataWrapper<T> Data)
        {
            UpdateQueue.Add(Data);
        }

        protected async Task UpdateData(ModuleDataWrapper<T> Data)
        {
            await DB.Upsert(CollectionName, Data.ID, Data.Data);
        }
        
        private ModuleDataWrapperInternal GenWrapper()
        {
            return InternalPool.TryDequeue(out var IWrapper) ? IWrapper : new ModuleDataWrapperInternal();
        }
        
        //Recycling wrappers to avoid GC
        
        protected async Task<ModuleDataWrapper<T>> TryGetData(BsonValue ID)
        {
            var IWrapper = GenWrapper();

           IWrapper.SetID(ID);

            if (Pool.TryGetValue(IWrapper, out var Data))
            {
                InternalPool.Enqueue(IWrapper);
                
                return Data;
            }

            var DataT = await DB.Get<T>(CollectionName, ID);

            if (DataT == null)
            {
                InternalPool.Enqueue(IWrapper);

                return null;
            }

            IWrapper.SetData(DataT);

            Pool.Add(IWrapper);
            
            return IWrapper;
        }

        protected IEnumerable<ModuleDataWrapper<T>> GetCollection()
        {
            foreach (var P in Pool)
            {
                yield return P;
            }
            
            var Col = DB.GetCollection<T>(CollectionName);

            if (Col == null)
            {
                yield break;
            }
            
            foreach (var C in Col)
            {
                var IWrapper = GenWrapper();
                
                IWrapper.SetID(C["_id"]);

                if (Pool.Contains(IWrapper))
                {
                    InternalPool.Enqueue(IWrapper);
                    
                    continue;
                }
                
                IWrapper.SetData(BsonSerializer.Deserialize<T>(C));

                Pool.Add(IWrapper);
                
                yield return IWrapper;
            }
        }

        protected ModuleDataWrapper<T> CreateData(BsonValue ID, T Data)
        {
            var IWrapper = GenWrapper();
            
            IWrapper.SetID(ID);
            
            IWrapper.SetData(Data);

            Pool.Add(IWrapper);

            UpdateQueue.Add(IWrapper);

            return IWrapper;
        }

        protected async Task DeleteData(BsonValue ID, ModuleDataWrapper<T> Data)
        {
            InternalPool.Enqueue((ModuleDataWrapperInternal)Data);
            
            Pool.Remove(Data);

            await DB.Delete<T>(CollectionName, ID);
        }
        
        protected async Task DeleteData(BsonValue ID)
        {
            var IWrapper = GenWrapper();
            
            IWrapper.SetID(ID);

            if (Pool.TryGetValue(IWrapper, out var Data))
            {
                Pool.Remove(Data);
                
                InternalPool.Enqueue((ModuleDataWrapperInternal)Data);
            }
            
            InternalPool.Enqueue(IWrapper);

            await DB.Delete<T>(CollectionName, ID);
        }
    }
}
