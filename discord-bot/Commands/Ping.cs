using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;

namespace discord_bot.Commands
{
    class Ping: ICommand
    {
        public async Task Handle(DSharpPlus.Entities.DiscordMessage msg)
        {
            await msg.RespondAsync("pong!");
        }
    }
}
