using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Commands;
using DiscordBot.Configurations;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace DiscordBot
{
    class Bot
    {
        private string Token { get; set; }
        private string CommandPrefix { get; set; }
        private Dictionary<string, ICommand> CommandDict { get; set; }
        private ICommandGetter Getter { get; set; }
        
        /// <summary>
        /// Receives messages and handles them
        /// </summary>
        /// <param name="args"></param>
        /// <param name="configs"></param>
        /// <returns></returns>
        public async Task Run(string[] args, Config configs)
        {
            Token = configs.Token;
            CommandPrefix = configs.Prefix;

            // Instantiate the bot
            var discord = new DiscordClient(new DiscordConfiguration
            {
                Token = Token,
                TokenType = TokenType.Bot
            });

            // We received a message
            discord.MessageCreated += MessageHandler;
            
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        /// <summary>
        /// Checks whether a message is a command (starts with the command prefix)
        /// </summary>
        /// <param name="e"></param>
        /// <param name="commandPrefix"></param>
        /// <returns></returns>
        private static bool IsCommand(MessageCreateEventArgs e, string commandPrefix)
        {
            return e.Message.Content.ToLower().StartsWith(commandPrefix);
        }

        private async Task MessageHandler(MessageCreateEventArgs e)
        {
            if (!IsCommand(e, CommandPrefix))
                return;
            
            ICommand command = Getter.Get(e.Message, CommandDict, CommandPrefix);
            if (command is null)
                return;
            
            await command.Run(e.Message);
        }
    }
}
