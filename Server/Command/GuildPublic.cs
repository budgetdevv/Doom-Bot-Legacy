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
    }
}
