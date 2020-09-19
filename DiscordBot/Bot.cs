using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DiscordBot.Commands;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot
{
    class Bot
    {
        private string Token { get;}
        private string CommandPrefix { get; set; }
        private ICommandProvider CommandProvider { get; set; }
        private DiscordClient DiscordClient { get; }
        
        public Bot(ICommandProvider commandProvider, Config.Config config)
        {
            CommandProvider = commandProvider;
            Token = config.Token;
            CommandPrefix = config.Prefix;
            
            // Instantiate the bot
            DiscordClient = new DiscordClient(new DiscordConfiguration
            {
                Token = Token,
                TokenType = TokenType.Bot
            });
        }
        
        
        /// <summary>
        /// Receives messages and handles them
        /// </summary>
        /// <returns></returns>
        public async Task Run()
        {
            // We received a message
            
            DiscordClient.MessageCreated += MessageHandler;

            await DiscordClient.ConnectAsync();
            await Task.Delay(-1);
        }

        /// <summary>
        /// Checks whether a message is a command (starts with the command prefix)
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private bool IsCommand(string content) =>
            content.ToLower().StartsWith(CommandPrefix);

        private async Task MessageHandler(MessageCreateEventArgs e)
        {
            ICommand command = CommandProvider.ProvideCommand(e.Message.Content);
            
            if (command is null)
            {
                foreach (var comm in CommandProvider.Commands.Values)
                {
                    await comm.OnMessage(e);
                }
            }
            
            if (command is null)
            {
                if (IsCommand(e.Message.Content))
                    await e.Message.RespondAsync("Command not recognized :(");
                return;
            }
            
            await command.Run(e.Message);
        }
    }
}