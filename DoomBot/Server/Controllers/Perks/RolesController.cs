using DoomBot.Server.Controllers.Attributes;
using DoomBot.Server.Managers;
using DoomBot.Server.Modules;
using DoomBot.Shared;
using DoomBot.Shared.Perks.Role;
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
    [Route("Perks/[controller]")]
    [RequireUserAccess]
    public class RolesController : ControllerBase
    {
        private UserAccessor UA;

        private RolePerkModule RPM;

        public RolesController(UserAccessor UA, RolePerkModule RPM)
        {
            this.UA = UA;

            this.RPM = RPM;
        }

        [HttpGet()]
        public async Task<RoleData> GetData()
        {
            using var Data = await RPM.GenRoleData(UA.AuthUser?.GUser);

            return Data;
        }

        [HttpPost()]
        public async Task<RoleData> PostData([FromBody] RoleData _RD)
        {
            using var RD = _RD;

            return await RPM.TrySetRoleData(UA.AuthUser.GUser, RD);
        }
    }
}
