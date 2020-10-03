using System;
using System.Threading.Tasks;
using DiscordBot.Backends;
using DiscordBot.Commands;
using DSharpPlus;

namespace DiscordBot
{
    class Bot<T>
    {
        private string Token { get;}
        private string CommandPrefix { get; set; }
        private ICommandProvider CommandProvider { get; set; }
        public DiscordClient DiscordClient { get; }
        
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
        public async Task Run(IBackend<T> backend)
        {
            await backend.Run();
        }

        /// <summary>
        /// Checks whether a message is a command (starts with the command prefix)
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private bool IsCommand(string content) =>
            content.ToLower().StartsWith(CommandPrefix);
 
        public async Task MessageHandler(IContext e)
        {
            ICommand command = CommandProvider.ProvideCommand(e.ExtractMessageContent());
            
            if (command is null)
            {
                foreach (var comm in CommandProvider.Commands.Values)
                {
                    await comm.OnMessage(e);
                }
            }
            
            if (command is null)
            {
                if (IsCommand(e.ExtractMessageContent()))
                    await e.Respond("Command not recognized :(");
                return;
            }
            
            await command.Run(e);
        }
    }
}