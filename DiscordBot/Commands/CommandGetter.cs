using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DiscordBot.Commands
{
    class CommandGetter: ICommandGetter
    {
        public Dictionary<string, ICommand> Commands { get; set; }
        public string CommandPrefix { get; }
        
        public CommandGetter(Dictionary<string, ICommand> commands,
            string commandPrefix)
        {
            Commands = commands;
            CommandPrefix = commandPrefix;
        }
        
        public ICommand Get(DiscordMessage msg)
        {
            string curCommand = CommandParser.Parse(msg, CommandPrefix)[0];

            ICommand command = null;
            
            try
            {
                command = Commands[curCommand];
            }
            catch (KeyNotFoundException) {}

            return command;
        }
    }
}
