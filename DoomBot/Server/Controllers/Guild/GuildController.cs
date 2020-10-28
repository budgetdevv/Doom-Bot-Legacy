using Discord;
using Discord.WebSocket;
using DoomBot.Server.Controllers.Attributes;
using DoomBot.Server.Modules;
using DoomBot.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoomBot.Server.Controllers.Perks
{

    [ApiController]
    [Route("[controller]")]
    public class GuildController: ControllerBase
    {
        private DiscordAccessor DA;

        private SocketGuild Guild;

        public GuildController(DiscordAccessor DA)
        {
            this.DA = DA;

            Guild = this.DA.CurrentGuild;
        }

        [HttpGet("Presence")]
        public (int Online, int Total) GetPresence()
        {
            return (Guild.Users.Count(x => x.Status != UserStatus.Offline), Guild.MemberCount);
        }
    }
}
