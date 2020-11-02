using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DoomBot.Server.Managers;

namespace DoomBot.Server.Command
{
    public class CommandHandlingService
    {
        private readonly IServiceProvider Services;
        private readonly CommandService Commands;
        private readonly DiscordSocketClient Client;
        private readonly EconManager EM;

        public CommandHandlingService(IServiceProvider Services)
        {
            this.Services = Services;

            Commands = Services.GetRequiredService<CommandService>();
            Client = Services.GetRequiredService<DiscordSocketClient>();
            EM = Services.GetRequiredService<EconManager>();


            // Hook CommandExecuted to handle post-command-execution logic.
            Commands.CommandExecuted += CommandExecutedAsync;
            // Hook MessageReceived so we can process each message to see
            // if it qualifies as a command.
            Client.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            // Register modules that are public and inherit ModuleBase<T>.
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), Services);
        }

        public async Task MessageReceivedAsync(SocketMessage RawMsg)
        {
            // Ignore system messages, or messages from other bots
            if (!(RawMsg is SocketUserMessage Msg)) return;
            if (Msg.Source != MessageSource.User) return;

            // This value holds the offset where the prefix ends
            var argPos = 0;
            // Perform prefix check. You may want to replace this with
            // (!message.HasCharPrefix('!', ref argPos))
            // for a more traditional command format like !help.
            if (!Msg.HasMentionPrefix(Client.CurrentUser, ref argPos) && !Msg.HasCharPrefix('!', ref argPos))
            {
                if (Msg.Channel is SocketTextChannel)
                {
                    _ = EM.OnText(Msg);
                }

                return;
            }

            var context = new SocketCommandContext(Client, Msg);
            // Perform the execution of the command. In this method,
            // the command service will perform precondition and parsing check
            // then execute the command if one is matched.
            await Commands.ExecuteAsync(context, argPos, Services); 
            // Note that normally a result will be returned by this format, but here
            // we will handle the result in CommandExecutedAsync,
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> Cmd, ICommandContext Context, IResult Result)
        {
            // command is unspecified when there was a search failure (command not found); we don't care about these errors
            if (!Cmd.IsSpecified)
                return;

            // the command was successful, we don't care about this result, unless we want to log that a command succeeded.
            if (Result.IsSuccess)
                return;

            // the command failed, let's notify the user that something happened.
            await Context.Channel.SendMessageAsync($"Error: {Result}");
        }
    }
}
