using Discord;
using Discord.WebSocket;
using DoomBot.Server.MongoDB;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DoomBot.Server.Modules
{
    public sealed class InventoryModule: DoomModuleBase<Inventory>
    {
        public InventoryModule(MDB DB): base ("Inventories", DB)
        {
            //Nothing here xd
        }

        //An Inventory instance is guaranteed

        public async Task<ModuleDataWrapper<Inventory>> GetOrCreateInv(long ID)
        {
            var Inv = await TryGetData(ID);

            return Inv ?? CreateData(ID, new Inventory(ID));
        }

        public async Task<ModuleDataWrapper<Inventory>> TryGetInv(long ID)
        {
            return await TryGetData(ID) ?? null;
        }

        public IEnumerable<ModuleDataWrapper<Inventory>> GetInvs()
        {
            return GetCollection();
        }
    }
}
