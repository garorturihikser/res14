using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace discord_bot.Commands
{
    // Commands that only send a message in response
    interface ICommand
    {
        Task Handle(DSharpPlus.Entities.DiscordMessage msg);
    }
}
