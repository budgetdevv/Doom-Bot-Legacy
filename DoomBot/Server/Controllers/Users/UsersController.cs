using Discord;
using Discord.WebSocket;
using DoomBot.Server.Controllers.Attributes;
using DoomBot.Server.Managers;
using DoomBot.Server.Modules;
using DoomBot.Shared;
using DoomBot.Shared.Perks;
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
    public class UsersController: ControllerBase
    {
        private UserAccessor UA;

        private DiscordAccessor DA;

        private PerksManager PM;

        public UsersController(UserAccessor UA, DiscordAccessor DA, PerksManager PM)
        {
            this.UA = UA;

            this.DA = DA;

            this.PM = PM;
        }

        [RequireUserAccess]
        [HttpGet("Data")]
        public (string Username, string AvatarURL, Perk[] Perks) GetUserData()
        {
            var User = UA.AuthUser.GUser;

            return (User.ToString(), User.GetAvatarUrl() ?? User.GetDefaultAvatarUrl(), PM.GetUserPerks(User));
        }

        [HttpGet("Data/{UserID}")]
        public (string Username, string AvatarURL, Perk[] Perks) GetUserData([FromRoute] ulong UserID)
        {
            var User = DA.GetUser(UserID);

            if (User == default)
            {
                return default;
            }

            return (User.ToString(), User.GetAvatarUrl() ?? User.GetDefaultAvatarUrl(), PM.GetUserPerks(User));
        }
    }
}
