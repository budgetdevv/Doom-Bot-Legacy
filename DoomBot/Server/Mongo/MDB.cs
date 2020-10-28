using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;   

namespace DoomBot.Server.MongoDB
{
    public class MDB
    {
        private IMongoDatabase DB;

        public MDB()
        {
            var DBClient = new MongoClient();

            DB = DBClient.GetDatabase("DoomBot");
        }

        public async Task Upsert<T>(string Type, BsonValue ID, T Data)
        {
            var Collection = DB.GetCollection<T>(Type);

            await Collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", ID), Data, new ReplaceOptions { IsUpsert = true });
        }

        public async Task Delete<T>(string Type, BsonValue ID)
        {
            var Collection = DB.GetCollection<T>(Type);

            var Filter = Builders<T>.Filter.Eq("_id", ID);

            await Collection.DeleteOneAsync(Filter);
        }

        public async Task<List<T>> GetAllMatch<T, F>(string Type, string MatchProp, F MatchValue)
        {
            var Collection = DB.GetCollection<T>(Type);

            var Filter = Builders<T>.Filter.Eq(MatchProp, MatchValue);

            return await (await Collection.FindAsync( Filter )).ToListAsync();
        }

        public async Task<T> Get<T>(string Type, BsonValue ID)
        {
            var Collection = DB.GetCollection<T>(Type);

            var Data = await (await Collection.FindAsync(Builders<T>.Filter.Eq("_id", ID))).FirstOrDefaultAsync();

            if (Data != null)
            {
                return Data;
            }

            return default;
        }

        public long CountCollectionEst<T>(string Type)
        {
            var Collection = DB.GetCollection<T>(Type);

            return Collection.EstimatedDocumentCount(); 
        }
    }
}
