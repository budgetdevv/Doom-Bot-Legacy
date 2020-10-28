using Discord;
using Discord.WebSocket;
using DoomBot.Server.Controllers.Attributes;
using DoomBot.Server.Managers;
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
    public class AuthController : ControllerBase
    {
        private AuthManager AM;

        private UserAccessor UA;

        public AuthController(AuthManager AM, UserAccessor UA)
        {
            this.AM = AM;

            this.UA = UA;
        }

        [HttpGet("{Bearer}")]
        public long Login(long Bearer)
        {
            return AM.Login(HttpContext, Bearer);
        }

        [RequireUserAccess]
        [HttpGet()]
        public void Logout(long Bearer)
        {
            AM.Logout(HttpContext, Bearer);
        }
    }
}
