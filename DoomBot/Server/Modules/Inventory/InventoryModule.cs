using Discord;
using Discord.WebSocket;
using DoomBot.Server.MongoDB;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoomBot.Server.Modules
{
    public sealed class InventoryModule: DoomModuleBase<Inventory>
    {
        public InventoryModule(MDB DB): base (DB, "Inventories")
        {
            //Nothing here xd
        }

        //An Inventory instance is guaranteed

        public async Task<MongoDataWrapper> GetOrCreateInv(long UserID)
        {
            var Inv = await GetData(UserID);

            if (Inv == default)
            {
                Inv = await CreateData(UserID, new Inventory(UserID));
            }

            return Inv;
        }

        public async Task<Inventory> GetOrCreateInvUnwrapped(long UserID)
        {
            var Inv = await GetDataUnwrapped(UserID);

            if (Inv == default)
            {
                Inv = await CreateDataUnwrapped(UserID, new Inventory(UserID));
            }

            return Inv;
        }

        public async Task<MongoDataWrapper> TryGetInv(long UserID)
        {
            var Inv = await GetData(UserID);

            if (Inv == default)
            {
                return default;
            }

            return Inv;
        }

        public async Task<Inventory> TryGetInvUnwrapped(long UserID)
        {
            var Inv = await GetDataUnwrapped(UserID);

            if (Inv == default)
            {
                return default;
            }

            return Inv;
        }

        public void DeleteInv(long UserID)
        {
            DeleteData(UserID);
        }
    }
}
