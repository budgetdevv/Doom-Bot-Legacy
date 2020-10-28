using Discord;
using Discord.API;
using Discord.Rest;
using Discord.WebSocket;
using DoomBot.Server.MongoDB;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoomBot.Server.Modules
{
    public sealed class DiscordAccessor
    {
        public readonly DiscordSocketClient Client;

        public readonly DiscordRestClient RClient;

        public SocketGuild CurrentGuild
        {
            get
            {
                if (_CurrentGuild == default)
                {
                    _CurrentGuild = Client.Guilds.FirstOrDefault();
                }

                return _CurrentGuild;
            }
        }

        private SocketGuild _CurrentGuild;

        public DiscordAccessor(DiscordSocketClient Client, DiscordRestClient RClient)
        {
            this.Client = Client;

            this.RClient = RClient;
        }

        public SocketGuildUser GetUser(ulong UserID)
        {
            return CurrentGuild.GetUser(UserID);
        }
    }
}
