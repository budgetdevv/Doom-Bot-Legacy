using DoomBot.Server.MongoDB;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoomBot.Server.Modules
{
    public abstract class DoomModuleBase<T>
    {
        public class MongoDataWrapper : IDisposable
        {
            //Static

            private static Queue<MongoDataWrapper> Recycled = new Queue<MongoDataWrapper>();

            public static MongoDataWrapper Create(T Data, Action<MongoDataWrapper, bool> DataUpdater)
            {
                if (Recycled.TryDequeue(out MongoDataWrapper W))
                {
                    W.Data = Data;

                    W.DataUpdater = DataUpdater;
                }

                else
                {
                    W = new MongoDataWrapper(Data, DataUpdater);
                }

                return W;
            }

            public static void Recycle(MongoDataWrapper Wrapper)
            {
                Wrapper.Disposed = false;

                Recycled.Enqueue(Wrapper);
            }

            //Instance

            public T Data;

            public bool Disposed;

            private Action<MongoDataWrapper, bool> DataUpdater;

            private MongoDataWrapper(T Data, Action<MongoDataWrapper, bool> DataUpdater)
            {
                this.Data = Data;

                this.DataUpdater = DataUpdater;
            }

            public void Update(bool Immediate = false)
            {
                DataUpdater.Invoke(this, false);
            }

            public void Dispose()
            {
                Disposed = true;

                Update();
            }
        }

        private readonly Dictionary<BsonValue, T> Pool;

        private readonly HashSet<(BsonValue ID, T Data)> UpdateQueue;

        private readonly MDB DB;

        private readonly string CollectionType;

        protected Action<Dictionary<BsonValue, T>> CustomDataPoolLoop;

        public DoomModuleBase(MDB _DB, string _CollectionType)
        {
            DB = _DB;

            Pool = new Dictionary<BsonValue, T>();

            UpdateQueue = new HashSet<(BsonValue ID, T Data)>();

            CollectionType = _CollectionType;

            _ = Task.Run(async () =>
            {
                //This never ends

                _ = Loop();
            });
        }

        protected async Task<MongoDataWrapper> GetData(BsonValue ID)
        {
            if (Pool.TryGetValue(ID, out T Data))
            {
                return WrapData(ID, Data);
            }

            Data = await DB.Get<T>(CollectionType, ID);

            if (Data == null)
            {
                return default;
            }

            //Add it to the pool so subsequent access can be faster

            Pool.TryAdd(ID, Data);

            return WrapData(ID, Data);
        }



        protected void DeleteData(BsonValue ID)
        {
            Pool.Remove(ID);

            _ = Task.Run(async () =>
            {
                await DB.Delete<T>(CollectionType, ID);
            });
        }

        private MongoDataWrapper WrapData(BsonValue ID, T Data)
        {
            return MongoDataWrapper.Create(Data, async (Wrapper, Immediate) => await WrapperUpdaterCallback(ID, Wrapper, Immediate));
        }

        private async Task WrapperUpdaterCallback(BsonValue ID, MongoDataWrapper Wrapper, bool Immediate)
        {
            await UpdateData(ID, Wrapper.Data, Immediate);

            if (Wrapper.Disposed)
            {
                MongoDataWrapper.Recycle(Wrapper);
            }
        }

        private async Task UpdateData(BsonValue ID, T Data, bool Immediate = false)
        {
            if (!Immediate)
            {
                UpdateQueue.Add((ID, Data));

                return;
            }

            await Task.Run(async () =>
            {
                await DB.Upsert(CollectionType, ID, Data);
            });
        }

        protected async Task<MongoDataWrapper> CreateData(BsonValue ID, T NewData)
        {
            Pool.TryAdd(ID, NewData);

            await UpdateData(ID, NewData, true);

            return WrapData(ID, NewData);
        }



        //Unwrapped

        protected async Task<T> GetDataUnwrapped(BsonValue ID)
        {
            if (Pool.TryGetValue(ID, out T Data))
            {
                return Data;
            }

            Data = await DB.Get<T>(CollectionType, ID);

            if (Data == null)
            {
                return default;
            }

            //Add it to the pool so subsequent access can be faster

            Pool.TryAdd(ID, Data);

            return Data;
        }

        protected async Task<T> CreateDataUnwrapped(BsonValue ID, T NewData)
        {
            Pool.TryAdd(ID, NewData);

            await UpdateData(ID, NewData, true);

            return NewData;
        }

        private async Task Loop()
        {
            while (true)
            {
                //Update stuff in Queue

                foreach (var Data in UpdateQueue)
                {
                    _ = UpdateData(Data.ID, Data.Data, true);
                }

                UpdateQueue.Clear();

                //Trim Pool if its too huge...

                if (CustomDataPoolLoop != default)
                {
                    CustomDataPoolLoop.Invoke(Pool);
                }

                await Task.Delay(10000);
            }
        }
    }
}
