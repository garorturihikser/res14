using System.Collections.Generic;

namespace DiscordBot.Commands
{
    interface ICommandProvider
    {
        public Dictionary<string[], ICommand> Commands { get; set; }
        
        ICommand ProvideCommand(string content);
    }
}
