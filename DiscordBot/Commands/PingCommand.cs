using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace DiscordBot.Commands
{
    class PingCommand: ICommand
    {
        public async Task Run(DiscordMessage msg)
        {
            await msg.RespondAsync("pong!");
        }
    }
}
