using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DoomBot.Server.Managers;
using DoomBot.Server.Modules;
using DoomBot.Shared.Perks;

namespace DoomBot.Server.Command
{
    // Modules must be public and inherit from an IModuleBase

    [RequireBotPermission(GuildPermission.Administrator)]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireContext(ContextType.Guild, ErrorMessage = "Sorry, this command must be ran from within a server, not a DM!")]
    public class GuildAdmin : ModuleBase<SocketCommandContext>
    {
        // Dependency Injection will fill this value in for us

        public AuthManager AM { get; set; }

        public PerksManager PM { get; set; }

        public EconManager EM { get; set; }

        public InventoryModule IM { get; set; }

        [Command("setperk", RunMode = RunMode.Async)]
        public async Task SetPerkRole(string Type, SocketRole Role)
        {
            if (!Enum.TryParse(Type, true, out Perk Result) || !Enum.IsDefined(Result))
            {
                _ = ReplyAsync($":negative_squared_cross_mark: | No such type - `{Type}` !");

                return;
            }

            PM.SetPerk(Result, Role);


            _ = ReplyAsync($":white_check_mark: | Successfully bound type - `{Type}` to {Role.Mention} !");
        }

        [Command("perks", RunMode = RunMode.Async)]
        public async Task Perks()
        {
            var SB = new StringBuilder();

            foreach (var P in Enum.GetNames(typeof(Perk)))
            {
                SB.Append($"\n{P}");
            }

            _ = ReplyAsync(SB.ToString());
        }

        [Command("setplaceholderrole", RunMode = RunMode.Async)]
        public async Task SetPHRole(SocketRole PH = default)
        {
            EM.SetPlaceHolderRole(PH);

            if (PH == default)
            {
                _ = ReplyAsync($":white_check_mark: | Cleared PlaceHolder role!");
            }

            else
            {
                _ = ReplyAsync($":white_check_mark: | {PH.Mention} is now the PlaceHolder role!");
            }
        }

        [Command("addspawnchannel", RunMode = RunMode.Async)]
        public async Task AddSpawnChannel(SocketTextChannel TC)
        {
            EM.AddSpawnChannel(TC);

            _ = ReplyAsync($":white_check_mark: | {TC.Mention} is now a Spawn Channel!");
        }

        [Command("removespawnchannel", RunMode = RunMode.Async)]
        public async Task RemoveSpawnChannel(SocketTextChannel TC)
        {
            EM.RemoveSpawnChannel(TC);

            _ = ReplyAsync($":white_check_mark: | {TC.Mention} is no longer a Spawn Channel!");
        }
    }
}
