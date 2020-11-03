using DoomBot.Server.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Discord.WebSocket;
using Discord;
using DoomBot.Server.Controllers.Attributes;
using DoomBot.Shared.Perks;

namespace DoomBot.Server.Managers
{
    public class PerksManager
    {
        private struct PerksConfig
        {
            public ulong RolePerkID, RoleCustomizerPerkID, TextChannelPerkID, VoiceChannelPerkID;
        }

        private PerksConfig PerksConf;

        private string Path => "PerksConf.json";

        private List<Perk> PerksCompiler;

        private Array Perks;

        public PerksManager()
        {
            if (!File.Exists(Path))
            {
                PerksConf = new PerksConfig();

                UpdateConf();
            }

            else
            {
                PerksConf = JsonConvert.DeserializeObject<PerksConfig>(File.ReadAllText(Path));
            }

            PerksCompiler = new List<Perk>();

            Perks = Enum.GetValues(typeof(Perk));
        }

        public void UpdateConf()
        {
            File.WriteAllText(Path, JsonConvert.SerializeObject(PerksConf));
        }

        public void SetPerk(Perk Perk, SocketRole Role)
        {
            switch (Perk)
            {
                default:
                    {
                        return;
                    }

                case Perk.RolePerk:
                    {
                        PerksConf.RolePerkID = Role.Id;

                        break;
                    }

                case Perk.RoleCustomizerPerk:
                    {
                        PerksConf.RoleCustomizerPerkID = Role.Id;

                        break;
                    }

                case Perk.TextChannelPerk:
                    {
                        PerksConf.TextChannelPerkID = Role.Id;

                        break;
                    }

                case Perk.VoiceChannelPerk:
                    {
                        PerksConf.VoiceChannelPerkID = Role.Id;

                        break;
                    }
            }

            UpdateConf();
        }

        public bool HasPerk(Perk Perk, SocketGuildUser GUser)
        {
            bool HasRole(ulong ID)
            {
                return GUser.Roles.Any(x => x.Id == ID);
            }

            switch (Perk)
            {
                default:
                    {
                        break;
                    }

                case Perk.RolePerk:
                    {
                        return HasRole(PerksConf.RolePerkID);
                    }

                case Perk.RoleCustomizerPerk:
                    {
                        return HasRole(PerksConf.RoleCustomizerPerkID);
                    }

                case Perk.TextChannelPerk:
                    {
                        return HasRole(PerksConf.TextChannelPerkID);
                    }

                case Perk.VoiceChannelPerk:
                    {
                        return HasRole(PerksConf.VoiceChannelPerkID);
                    }
            }

            return false;
        }

        public Perk[] GetUserPerks(SocketGuildUser User)
        {
            PerksCompiler.Clear();

            foreach (Perk P in Perks)
            {
                if (HasPerk(P, User))
                {
                    PerksCompiler.Add(P);
                }
            }

            return PerksCompiler.ToArray();
        }
    }
}
