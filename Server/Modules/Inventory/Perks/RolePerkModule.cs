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
        private readonly DiscordAccessor DA;

        private InventoryModule IM;

        private readonly PerksManager PM;

        public RolePerkModule(DiscordAccessor DA, InventoryModule IM, PerksManager PM, MDB DB) : base("PerkRoles", DB)
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

        public async Task<ModuleDataWrapper<PerkRole>> TryGetRole(SocketGuildUser User)
        {
            if (!EligibleForRole(User))
            {
                return null;
            }

            long UserID = (long)User.Id;

            var PerksRole = await TryGetData(UserID);

            return PerksRole ?? null;
        }

        public async Task<ModuleDataWrapper<PerkRole>> CreateRole(SocketGuildUser User, RoleData Data)
        {
            long UserID = (long)User.Id;

            bool CanCustomize = CanCustomizeRole(User);

            //var NewRole = await Task.Run(async () => await DA.CurrentGuild.CreateRoleAsync(Data.Name, default, CanCustomize ? new Color(Data.Color.R, Data.Color.G, Data.Color.B) : default, CanCustomize ? Data.Hoisted : false, default));

            var NewRole = await Task.Run(async () => await DA.CurrentGuild.CreateRoleAsync("New Role", default, default, default, default));

            var PerksRole = CreateData(UserID, new PerkRole(UserID, NewRole.Id));

            return PerksRole;
        }

        public async Task<RoleData> GenRoleData(SocketGuildUser User)
        {
            var Data = await TryGetData((long)User.Id);

            if (Data == default)
            {
                return default;
            }

            var Role = TryGetSocketRole(User, Data.Data.RoleID);

            if (Role == default)
            {
                return default;
            }

            var RD = Recycler<RoleData>.Gen();

            WriteRoleData(User, Role, RD);

            return RD;
        }

        private static RoleData WriteRoleData(SocketGuildUser User, SocketRole Role, RoleData Data)
        {
            Data.Name = Role.Name;

            //Data.Color = (Role.Color.R, Role.Color.G, Role.Color.B);

            Data.Color = Role.Color.RawValue;

            Data.Hoisted = Role.IsHoisted;

            Data.Enabled = Role.Members.Contains(User);

            return Data;
        }

        public SocketRole TryGetSocketRole(SocketGuildUser User, ulong ID)
        {
            var Role = User.Guild.GetRole(ID);

            if (Role == default)
            {
                _ = DeleteData((long)User.Id);

                return default;
            }

            return Role;
        }

        public async Task<RoleData> TryApplyRoleData(SocketGuildUser User, RoleData RoleData)
        {
            if (!EligibleForRole(User))
            {
                return default;
            }

            using var Data = await TryGetData((long)User.Id);

            ulong RoleID = Data?.Data.RoleID ?? default;

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
                    await Role.ModifyAsync(x =>
                    {
                        x.Name = RoleData.Name;
                        x.Color = new Color(RoleData.Color);
                        x.Hoist = RoleData.Hoisted;
                    });
                }
            }

            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
            
            WriteRoleData(User, Role, RoleData);

            return RoleData;
        }
    }
}
