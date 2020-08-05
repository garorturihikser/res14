using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;

namespace discord_bot.Commands
{
    interface ISortCommand
    {
        Task Sort(DSharpPlus.Entities.DiscordMessage msg,
            Dictionary<string, ICommand> commands,
            string commandPrefix);
    }
}
