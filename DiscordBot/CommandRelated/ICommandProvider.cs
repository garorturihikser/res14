using System.Collections.Generic;
using DiscordBot.CommandRelated.Commands;

namespace DiscordBot.CommandRelated
{
    interface ICommandProvider
    {
        Dictionary<string[], ICommand> Commands { get; set; }
        
        ICommand ProvideCommand(string content);
    }
}
