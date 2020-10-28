using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DoomBot.Server.Managers;

namespace DoomBot.Server.Command
{
    // Modules must be public and inherit from an IModuleBase

    [RequireContext(ContextType.DM, ErrorMessage = "Sorry, this command must be ran from within a DM, not a Server!")]
    public class DMPublic : ModuleBase<SocketCommandContext>
    {

        [Command("clear", RunMode = RunMode.Async)]
        public async Task Clear()
        {
            var Msgs = await Context.Channel.GetMessagesAsync(200).FlattenAsync();

            foreach (var Msg in Msgs)
            {
                if (Msg.Author.Id != Context.Client.CurrentUser.Id)
                {
                    continue;
                }

                await Msg.DeleteAsync();
            }
        }
    }
}
