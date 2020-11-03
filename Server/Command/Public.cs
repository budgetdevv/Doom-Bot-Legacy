using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DoomBot.Server.EM;
using DoomBot.Server.Managers;
using DoomBot.Server.Modules;

namespace DoomBot.Server.Command
{
    // Modules must be public and inherit from an IModuleBase

    public class Public : ModuleBase<SocketCommandContext>
    {
        // Dependency Injection will fill this value in for us

        // ReSharper disable MemberCanBePrivate.Global
        
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        
        public AuthManager AM { get; set; }
        
        public InventoryModule IM { get; set; }
        

        public DiscordAccessor DA { get; set; }


        public EMManager EMM { get; set; }

        // ReSharper restore MemberCanBePrivate.Global
        
        // ReSharper restore UnusedAutoPropertyAccessor.Global
        
        [Command("logmein", RunMode = RunMode.Async)]
        public async Task LogMeIn()
        {
            var GUser = DA.CurrentGuild.GetUser(Context.User.Id);

            var Data = AM.GenBearer(GUser);

            if (Data.Bearer == default)
            {
                _ = ReplyAsync(":negative_squared_cross_mark: | You have a currently active login link in DMs!");

                return;
            }

            var Embed = new EmbedBuilder();

            Embed.Title = $"Login as {GUser}";

            Embed.Description = $"Click [HERE](https://kings-queens.github.io/Bearer/{Data.UserID}/{Data.Bearer}) to login! One-time use link, expires within 5 mins...";

            var T = GUser.SendMessageAsync(default, false, Embed.Build());

            while (T.IsCompleted)
            {
                await Task.Delay(1);
            }

            if (T.IsFaulted)
            {
                AM.InvalidateBearer(Data.UserID);

                _ = ReplyAsync($":negative_squared_cross_mark: | Operation has been cancelled as the bot couldn't DM you!");
            }
        }

        [Command("balance", RunMode = RunMode.Async)]
        public async Task Balance(IUser User = default)
        {
            User ??= Context.User;

            _ = ReplyAsync($":moneybag: | {User.Mention}'s balance is ${(await IM.GetOrCreateInv((long) User.Id)).Data.Cash}");
        }
        
        [Command("lbcash", RunMode = RunMode.Async)]
        public async Task LBCash()
        {
            bool Success = EMM.Gen(Context.User, Context.Channel, async (EM) =>
            {
                EM.Title = "Cash Leaderboard";

                EM.Desc = "Oh yes";
                
                foreach (var Data in IM.GetInvs().OrderByDescending(x => x.Data.Cash))
                {
                    var Inv = Data.Data;
                    
                    EM.AddElement(default, Context.Client.GetUser((ulong)Inv.ID)?.ToString() ?? "User left Guild!", $"${Inv.Cash}", default);
                }
                
                return true;
            });
            
        }
    }
}
