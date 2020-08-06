using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DiscordBot.Commands
{
    class CommandGetter: ICommandGetter
    {
        public ICommand Get(DiscordMessage msg,
            Dictionary<string, ICommand> commands,
            string commandPrefix)
        {
            string curCommand = CommandParser.Parse(msg, commandPrefix)[0];
            ICommand command = null;
            
            try
            {
                command = commands[curCommand];
            }
            catch (KeyNotFoundException) {}

            return command;
        }
    }
}
