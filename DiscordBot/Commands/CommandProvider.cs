using System.Collections.Generic;
using System.Linq;

namespace DiscordBot.Commands
{
    class CommandProvider: ICommandProvider
    {
        public Dictionary<string[], ICommand> Commands { get; set; }
        public string CommandPrefix { get; }
        
        public CommandProvider(Dictionary<string[], ICommand> commands,
            string commandPrefix)
        {
            Commands = commands;
            CommandPrefix = commandPrefix;
        }
        
        public ICommand ProvideCommand(string content)
        {
            string curCommand = CommandParser.Parse(content, CommandPrefix)[0];

            try
            {
                foreach (string[] commands in Commands.Keys)
                {
                    if (commands.All(x => x != curCommand)) continue;
                    var command = Commands[commands];
                    return command;
                }
            }
            catch (KeyNotFoundException) {}

            return null;
        }
    }
}
