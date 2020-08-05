using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;

namespace discord_bot.Commands
{
    class SortCommand: ISortCommand
    {
        public async Task Sort(DSharpPlus.Entities.DiscordMessage msg,
            Dictionary<string, ICommand> commands,
            string commandPrefix)
        {
            string[] content = msg.Content.ToLower().Split();
            string curCommand = content[0];
            curCommand = curCommand.Replace(commandPrefix, "");

            try
            {
                ICommand command = commands[curCommand];
                await command.Handle(msg);
            }
            catch (KeyNotFoundException e)
            {
                await msg.RespondAsync("Command not recgonized");
            }
        }
    }
}
