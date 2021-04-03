using System;
using System.Threading.Tasks;
using DiscordBot.BackendRelated;
using DiscordBot.CommandRelated;
using DiscordBot.CommandRelated.Commands;
using DSharpPlus;

namespace DiscordBot
{
    class Bot<T>
    {
        private string CommandPrefix { get; set; }
        private ICommandProvider CommandProvider { get; set; }
        public IBackend<T> Backend { get; set; }
        
        public Bot(ICommandProvider commandProvider, IBackend<T> backend, Config.DiscordConfig discordConfig)
        {
            CommandProvider = commandProvider;
            Backend = backend;
            CommandPrefix = discordConfig.Prefix;
        }

        /// <summary>
        /// Receives messages and handles them
        /// </summary>
        /// <returns></returns>
        public async Task Run()
        {
            await Backend.Run(MessageHandler);
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
                    await comm.OnRegularMessage(e);
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