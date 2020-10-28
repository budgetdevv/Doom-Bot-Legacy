using Discord.WebSocket;
using Microsoft.AspNetCore.Http;
using Snowflake;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace DoomBot.Server.Managers
{
    public class TokenComparer : IEqualityComparer<(string DevName, long Token)>
    {
        public bool Equals((string DevName, long Token) X, (string DevName, long Token) Y)
        {
            return (X.Token == Y.Token);
        }

        public int GetHashCode((string DevName, long Token) Obj)
        {
            return Obj.Token.GetHashCode();
        }
    }

    public class AuthUser
    {
        static AuthUser()
        {
            Comparer = new TokenComparer();
        }

        public static TokenComparer Comparer;

        public SocketGuildUser GUser;

        private Queue<long> Bearer;

        private HashSet<(string DevName, long Token)> ActiveTokens;

        private SnowflakeEngine SE;

        public AuthUser(SocketGuildUser GUser, SnowflakeEngine SE)
        {
            this.GUser = GUser;

            this.Bearer = new Queue<long>();

            ActiveTokens = new HashSet<(string DevName, long Token)>(Comparer);

            this.SE = SE;
        }

        public long GenBearer()
        {
            if (this.Bearer.Count == 1)
            {
                return default;
            }

            long Bearer = SE.Gen();

            this.Bearer.Enqueue(Bearer);

            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromMinutes(5));

                if (this.Bearer.Count == 1 && this.Bearer.Peek() == Bearer)
                {
                    InvalidateBearer();
                }
            });

            return Bearer;
        }

        public long GetToken(HttpContext HC, long Bearer)
        {
            if (this.Bearer.Count == 0 || this.Bearer.Peek() != Bearer)
            {
                return default;
            }

            this.Bearer.Dequeue();

            long Token = SE.Gen();

            ActiveTokens.Add((HC.Request.Headers["User-Agent"], Token));

            return Token;
        }

        public bool RevokeToken(long Token)
        {
            ActiveTokens.Remove((null, Token));

            return (ActiveTokens.Count == 0);
        }

        public bool IsValidToken(long Token)
        {
            return ActiveTokens.Contains((null, Token));
        }

        public void InvalidateBearer()
        {
            Bearer.Dequeue();
        }
    }
}
