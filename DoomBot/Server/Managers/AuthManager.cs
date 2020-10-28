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
        private HashSet<(long UserID, AuthUser AuthUser)> LoggedInUsers;

        private SnowflakeEngine SE;

        public AuthManager(SnowflakeEngine SE)
        {
            LoggedInUsers = new HashSet<(long UserID, AuthUser GUser)>(new LoggedInComparer());

            this.SE = SE;
        }

        public (long UserID, AuthUser GUser) TryAuth(HttpContext Context)
        {
            if (!long.TryParse(Context.Request.Headers["AuthToken"], out long Token) || !long.TryParse(Context.Request.Headers["UserID"], out long UserID) || !LoggedInUsers.TryGetValue((UserID, null), out var Data) || !Data.AuthUser.IsValidToken(Token))
            {
                return default;
            }

            if (Data.AuthUser.GUser == default)
            {
                LoggedInUsers.Remove(Data);

                return default;
            }

            return Data;
        }

        public (long UserID, long Bearer) GenBearer(SocketGuildUser GUser)
        {
            long UserID = (long)GUser.Id;

            if (LoggedInUsers.TryGetValue((UserID, null), out var Data))
            {
                return (UserID, Data.AuthUser.GenBearer());
            }

            else
            {
                Data = (UserID, new AuthUser(GUser, SE));

                LoggedInUsers.Add(Data);

                return (UserID, Data.AuthUser.GenBearer());
            }
        }

        public long Login(HttpContext HC, long Bearer)
        {
            if (!long.TryParse(HC.Request.Headers["UserID"], out long UserID))
            {
                return default;
            }

            if (!LoggedInUsers.TryGetValue((UserID, null), out var Data))
            {
                return default;
            }

            //Console.WriteLine($"Logging in - {UserID}");

            return Data.AuthUser.GetToken(HC, Bearer);
        }

        public void Logout(HttpContext HC, long Token)
        {
            if (!long.TryParse(HC.Request.Headers["UserID"], out long UserID))
            {
                return;
            }

            if (!LoggedInUsers.TryGetValue((UserID, null), out var Data))
            {
                return;
            }

            //If all devices are logged out

            if (Data.AuthUser.RevokeToken(Token))
            {
                LoggedInUsers.Remove(Data);
            }
        }

        public void InvalidateBearer(long UserID)
        {
            if (LoggedInUsers.TryGetValue((UserID, null), out var Data))
            {
                Data.AuthUser.InvalidateBearer();
            }
        }
    }

    public class LoggedInComparer : IEqualityComparer<(long UserID, AuthUser AuthUser)>
    {
        public bool Equals((long UserID, AuthUser AuthUser) X, (long UserID, AuthUser AuthUser) Y)
        {
            return (X.UserID == Y.UserID);
        }

        public int GetHashCode((long UserID, AuthUser AuthUser) Obj)
        {
            return Obj.UserID.GetHashCode();
        }
    }
}
