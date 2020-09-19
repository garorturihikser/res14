using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DiscordBot.Commands
{
    interface ICommandProvider
    {
        public Dictionary<string[], ICommand> Commands { get; set; }
        
        ICommand ProvideCommand(string content);
    }
}
