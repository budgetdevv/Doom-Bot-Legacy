using Discord;
using Discord.Rest;
using Discord.WebSocket;
using DoomBot.Server.Controllers.Attributes;
using DoomBot.Server.Managers;
using DoomBot.Server.MongoDB;
using DoomBot.Shared.Perks;
using DoomBot.Shared.Perks.Role;
using MongoDB.Bson;
using Recycler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoomBot.Server.Modules
{
    public sealed class RolePerkModule : DoomModuleBase<PerkRole>
    {
        private DiscordAccessor DA;

        private InventoryModule IM;

        private PerksManager PM;

        public RolePerkModule(DiscordAccessor DA, InventoryModule IM, PerksManager PM, MDB DB) : base(DB, "PerkRoles")
        {
            this.DA = DA;

            this.IM = IM;

            this.PM = PM;
        }

        private bool EligibleForRole(SocketGuildUser User)
        {
            return PM.HasPerk(Perk.RolePerk, User);
        }

        private bool CanCustomizeRole(SocketGuildUser User)
        {
            return PM.HasPerk(Perk.RoleCustomizerPerk, User);
        }

        public async Task<MongoDataWrapper> TryGetRole(SocketGuildUser User)
        {
            if (!EligibleForRole(User))
            {
                return default;
            }

            long UserID = (long)User.Id;

            var PerksRole = await GetData(UserID);

            if (PerksRole == default)
            {
                return default;
            }

            return PerksRole;
        }

        public async Task<PerkRole> TryGetRoleUnwrapped(SocketGuildUser User)
        {
            if (!EligibleForRole(User))
            {
                return default;
            }

            Console.WriteLine("Eli");

            long UserID = (long)User.Id;

            var PerksRole = await GetDataUnwrapped(UserID);

            if (PerksRole == default)
            {
                return default;
            }

            return PerksRole;
        }

        public async Task<MongoDataWrapper> CreateRole(SocketGuildUser User, RoleData Data)
        {
            long UserID = (long)User.Id;

            bool CanCustomize = CanCustomizeRole(User);

            //var NewRole = await Task.Run(async () => await DA.CurrentGuild.CreateRoleAsync(Data.Name, default, CanCustomize ? new Color(Data.Color.R, Data.Color.G, Data.Color.B) : default, CanCustomize ? Data.Hoisted : false, default));

            var NewRole = await Task.Run(async () => await DA.CurrentGuild.CreateRoleAsync("New Role", default, default, default, default));

            var PerksRole = await CreateData(UserID, new PerkRole(UserID, NewRole.Id));

            return PerksRole;
        }

        public void DeleteRole(SocketGuildUser User)
        {
            DeleteData((long)User.Id);
        }

        public async Task<RoleData> GenRoleData(SocketGuildUser User)
        {
            var Data = await GetDataUnwrapped((long)User.Id);

            if (Data == default)
            {
                return default;
            }

            var Role = TryGetSocketRole(User, Data.RoleID);

            if (Role == default)
            {
                return default;
            }

            var RD = Recycler<RoleData>.Gen();

            WriteRoleData(User, Role, RD);

            return RD;
        }

        public RoleData WriteRoleData(SocketGuildUser User, SocketRole Role, RoleData Data)
        {
            Data.Name = Role.Name;

            //Data.Color = (Role.Color.R, Role.Color.G, Role.Color.B);

            Data.Color = Role.Color;

            Data.Hoisted = Role.IsHoisted;

            Data.Enabled = Role.Members.Contains(User);

            return Data;
        }

        public SocketRole TryGetSocketRole(SocketGuildUser User, ulong ID)
        {
            var Role = User.Guild.GetRole(ID);

            if (Role == default)
            {
                DeleteData((long)User.Id);

                return default;
            }

            return Role;
        }

        public async Task<RoleData> TrySetRoleData(SocketGuildUser User, RoleData RoleData)
        {
            if (EligibleForRole(User))
            {
                var Data = await GetDataUnwrapped((long)User.Id);

                ulong RoleID = (Data == default) ? default : Data.RoleID;

                var Role = (RoleID == default) ? default : TryGetSocketRole(User, RoleID);

                if (Role == default)
                {
                    using var NewData = await CreateRole(User, RoleData);

                    Role = TryGetSocketRole(User, NewData.Data.RoleID);
                }

                try
                {
                        if (RoleData.Enabled)
                        {
                            await User.AddRoleAsync(Role);
                        }

                        else
                        {
                            await User.RemoveRoleAsync(Role);
                        }

                        if (CanCustomizeRole(User))
                        {
                            await Role.ModifyAsync(x => { x.Name = RoleData.Name; x.Color = new Color(RoleData.Color.R, RoleData.Color.G, RoleData.Color.B); x.Hoist = RoleData.Hoisted; });
                        }
                }

                catch
                {

                }

                WriteRoleData(User, Role, RoleData);

                return RoleData;
            }

            return default;
        }
    }
}
