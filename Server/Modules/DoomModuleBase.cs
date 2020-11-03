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
        
        private readonly Dictionary<BsonValue, ModuleDataWrapper<T>> Pool;

        private readonly HashSet<ModuleDataWrapper<T>> UpdateQueue;

        protected DoomModuleBase(string CollectionName, MDB DB)
        {
            ModuleDataWrapperInternal.SetCallback(WrapperCallback);
            
            this.CollectionName = CollectionName;
            
            this.DB = DB;

            Pool = new Dictionary<BsonValue, ModuleDataWrapper<T>>();
            
            UpdateQueue = new HashSet<ModuleDataWrapper<T>>();

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
                    TaskList.Add(Task.Run(async () => await UpdateData(Data, false)));
                }

                await Task.WhenAll(TaskList);
                
                TaskList.Clear();
                
                await Task.Delay(10000);
            }
        }
        
        private void WrapperCallback(ModuleDataWrapper<T> Data)
        {
            UpdateQueue.Add(Data);
        }

        protected async Task UpdateData(ModuleDataWrapper<T> Data, bool IsUpsert)
        {
            await DB.Update(CollectionName, Data.ID, Data.Data, IsUpsert);
        }

        //Recycling wrappers to avoid GC
        
        protected async Task<ModuleDataWrapper<T>> TryGetData(BsonValue ID)
        {
            if (Pool.TryGetValue(ID, out var Data))
            {
                return Data;
            }

            var RawData = await DB.Get<T>(CollectionName, ID);

            if (RawData == null)
            {
                return default;
            }
            
            var IWrapper = new ModuleDataWrapperInternal();

            IWrapper.SetID(ID);
            
            IWrapper.SetData(RawData);

            return IWrapper; 
        }

        protected IEnumerable<ModuleDataWrapper<T>> GetCollection()
        {
            foreach (var P in Pool.Values)
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
                var ID = C["_id"];
                
                if (Pool.ContainsKey(ID))
                {
                    continue;
                }
                
                var IWrapper = new ModuleDataWrapperInternal();
                
                IWrapper.SetID(ID);
                
                IWrapper.SetData(BsonSerializer.Deserialize<T>(C));

                Pool.Add(ID, IWrapper);
                
                yield return IWrapper;
            }
        }

        protected ModuleDataWrapper<T> CreateData(BsonValue ID, T Data)
        {
            var IWrapper = new ModuleDataWrapperInternal();
            
            IWrapper.SetID(ID);
            
            IWrapper.SetData(Data);

            Pool.Add(ID, IWrapper);

            _ = UpdateData(IWrapper, true);

            return IWrapper;
        } 

        protected async Task DeleteData(BsonValue ID)
        {
            Pool.Remove(ID);

            await DB.Delete<T>(CollectionName, ID);
        }
    }
}
