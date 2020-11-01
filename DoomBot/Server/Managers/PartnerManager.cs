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
using Discord.Commands;

namespace DoomBot.Server.Managers
{
    public class PartnerManager
    {
        private EmbedBuilder EM;

        public PartnerManager()
        {
            EM = new EmbedBuilder();
        }

        public async Task PartnerInit(SocketCommandContext Context)
        {
            var Msg = await Context.Channel.SendMessageAsync("Input invite link!");

            Context.Client.MessageReceived += OnMsgReceive;

            bool Cancelled = false;

            _ = Task.Run(async () =>
            {
                await Task.Delay(10000);

                if (!Cancelled)
                {
                    Context.Client.MessageReceived -= OnMsgReceive;
                }
            });

            async Task OnMsgReceive(SocketMessage Link)
            {
                if (Link.Author.Id != Context.User.Id)
                {
                    return;
                }

                string Msg = Link.Content;

                int I;

                for (I = Msg.Length - 1; I >= 0; I--)
                {
                    if (Msg[I] == '/')
                    {
                        Msg = Msg.AsSpan(I).ToString();

                        break;
                    }
                }

                if (I == 0)
                {

                }

                else
                {

                }

                Cancelled = true;

                Context.Client.MessageReceived -= OnMsgReceive;
            }
        }
    }
}
