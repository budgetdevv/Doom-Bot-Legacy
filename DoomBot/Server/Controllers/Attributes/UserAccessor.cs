using Discord;
using Discord.WebSocket;

namespace DoomBot.Server.Controllers.Attributes
{
    public class UserAccessor
    {
        public SocketGuildUser User { get; private set; }

        public long Token { get; private set; }

        public void ReadUserAs(SocketGuildUser User, long Token)
        {
            this.User = User;

            this.Token = Token;
        }
    }
}