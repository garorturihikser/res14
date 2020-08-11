using System.Threading.Tasks;
using DSharpPlus.Entities;

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
