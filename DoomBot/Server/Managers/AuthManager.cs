using DoomBot.Server.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Discord.WebSocket;
using Discord;
using Microsoft.AspNetCore.Http;
using Snowflake;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace DoomBot.Server.Managers
{
    public class AuthManager
    {
        private Dictionary<long, SocketGuildUser> LoggedInUsers;

        private HashSet<(long Bearer, SocketGuildUser User)> ValidBearer;

        private HashSet<SocketGuildUser> LockedUsers;

        private SnowflakeEngine SE;

        public AuthManager(SnowflakeEngine SE)
        {
            LoggedInUsers = new Dictionary<long, SocketGuildUser>();

            ValidBearer = new HashSet<(long Bearer, SocketGuildUser User)>(new ValidBearerComparer());

            LockedUsers = new HashSet<SocketGuildUser>();

            this.SE = SE;
        }

        public (SocketGuildUser GUser, long Token) TryAuth(HttpContext Context)
        {
            if (!long.TryParse(Context.Request.Headers["AuthToken"], out long Token) || !LoggedInUsers.TryGetValue(Token, out SocketGuildUser User) || User == default)
            {
                return default;
            }

            return (User, Token);
        }

        public long GenBearer(SocketGuildUser GUser)
        {
            if (LockedUsers.Contains(GUser))
            {
                return 0;
            }

            LockedUsers.Add(GUser);

            long Bearer = SE.Gen();

            var Stuff = (Bearer, GUser);

            ValidBearer.Add(Stuff);

            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromMinutes(5));

                InvalidateBearer(Bearer, GUser);
            });

            return Bearer;
        }

        public void InvalidateBearer(long Bearer, SocketGuildUser User)
        {
            ValidBearer.Remove((Bearer, User));

            LockedUsers.Remove(User);
        }

        public long Login(long Bearer)
        {
            if (!ValidBearer.TryGetValue((Bearer, null), out var Data))
            {
                return default;
            }

            InvalidateBearer(Bearer, Data.User);

            long Token = SE.Gen();

            LoggedInUsers.Add(Token, Data.User);

            return Token;
        }

        public void Logout(long Bearer)
        {
            LoggedInUsers.Remove(Bearer);
        }
    }

    public class ValidBearerComparer : IEqualityComparer<(long Bearer, SocketGuildUser User)>
    {
        public bool Equals((long Bearer, SocketGuildUser User) X, (long Bearer, SocketGuildUser User) Y)
        {
            return (X.Bearer == Y.Bearer);
        }

        public int GetHashCode((long Bearer, SocketGuildUser User) Obj)
        {
            return Obj.Bearer.GetHashCode();
        }
    }
}
