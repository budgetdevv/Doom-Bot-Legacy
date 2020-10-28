using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DoomBot.Server.Managers;
using DoomBot.Server.Modules;

namespace DoomBot.Server.Command
{
    // Modules must be public and inherit from an IModuleBase

    [RequireBotPermission(GuildPermission.Administrator)]
    [RequireContext(ContextType.Guild, ErrorMessage = "Sorry, this command must be ran from within a server, not a DM!")]
    public class GuildPublic : ModuleBase<SocketCommandContext>
    {
        // Dependency Injection will fill this value in for us

        public AuthManager AM { get; set; }

        public InventoryModule IM { get; set; }

        [Command("logmein", RunMode = RunMode.Async)]
        public async Task UserInfoAsync(IUser user = null)
        {
            var GUser = Context.Guild.GetUser(Context.User.Id);

            long Bearer = AM.GenBearer(GUser);

            if (Bearer == 0)
            {
                _  = ReplyAsync(":negative_squared_cross_mark: | You have a currently active login link in DMs!");

                return;
            }

            var Embed = new EmbedBuilder();

            Embed.Title = $"Login as {GUser}";

            Embed.Description = $"Click [HERE](https://kings-queens.github.io/Bearer/{Bearer}) to login! One-time use link, expires within 5 mins...";

            var T = GUser.SendMessageAsync(default, false, Embed.Build());

            while (T.IsCompleted)
            {
                await Task.Delay(1);
            }

            if (T.IsFaulted)
            {
                AM.InvalidateBearer(Bearer, GUser);

                _ = ReplyAsync($":negative_squared_cross_mark: | Operation has been cancelled as the bot couldn't DM you!");
            }
        }

        [Command("balance", RunMode = RunMode.Async)]
        public async Task Balance(SocketGuildUser User = default)
        {
            if (User == default)
            {
                User = Context.Guild.GetUser(Context.User.Id);
            }

            _ = ReplyAsync($":moneybag: | {User.Mention}'s balance is ${(await IM.GetOrCreateInvUnwrapped((long)User.Id)).Cash}");
        }
    }
}
