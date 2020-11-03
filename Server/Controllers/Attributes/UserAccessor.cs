using Discord;
using Discord.WebSocket;
using DoomBot.Server.Managers;

namespace DoomBot.Server.Controllers.Attributes
{
    public class UserAccessor
    {
        public AuthUser AuthUser { get; private set; }

        public void ReadUserAs(AuthUser User)
        {
            this.AuthUser = User;
        }
    }
}