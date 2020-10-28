using System.Collections.Generic;

namespace DoomBot.Server.Managers
{
    public class AuthUser
    {
        public ulong UserID;

        public List<(string DevName, long Token)> ActiveTokens;

        public AuthUser(ulong UserID)
        {
            this.UserID = UserID;

            ActiveTokens = new List<(string DevName, long Token)>();
        }
        public void RegisterToken(string DevName, long Token)
        {
            ActiveTokens.Add((DevName, Token));
        }

        public void RevokeToken(long Token)
        {
            ActiveTokens.RemoveAll(x => x.Token == Token);
        }
    }
}
