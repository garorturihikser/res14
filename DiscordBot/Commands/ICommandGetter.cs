using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DiscordBot.Commands
{
    interface ICommandGetter
    {
        ICommand Get(DiscordMessage msg);
    }
}
