using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Commands;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace DiscordBot
{
    class Bot
    {
        private string Token { get;}
        private string CommandPrefix { get; set; }
        private ICommandGetter CommandGetter { get; set; }
        private DiscordClient DiscordClient { get; }
        
        public Bot(ICommandGetter commandGetter, Config.Config config)
        {
            CommandGetter = commandGetter;
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
            DiscordClient.MessageCreated += HandleTicTacToeScreen;
            
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
            if (!IsCommand(e.Message.Content))
                return;

            ICommand command = CommandGetter.Get(e.Message.Content);

            if (command is null)
            {
                await e.Message.RespondAsync("Command not recognized :(");
                return;
            }
            
            await command.Run(e.Message);
        }

        public async Task HandleTicTacToeScreen(MessageCreateEventArgs e)
        {
            if (!IsTicTacToeScreen(e.Message.Content))
                return;
            
            ICommand command = CommandGetter.Get("j!tic");

            await command.Run(e.Message);
        }
        
        public bool IsTicTacToeScreen(string content)
        {
            var ticTacToeScreenHeight = 3;
            var ticTacToeScreenWidth = 3;
            var numOfNewLines = 2;

            foreach (char chr in content)
            {
                if (chr != '❌' && chr != '⭕' && chr != '⬜' && chr != '\n')
                    return false;
            }

            return ticTacToeScreenHeight * ticTacToeScreenWidth + numOfNewLines == content.Length;
        }
    }
}