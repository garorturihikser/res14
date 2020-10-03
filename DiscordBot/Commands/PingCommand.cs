using System.Threading.Tasks;
using DiscordBot.Backends;

namespace DiscordBot.Commands
{
    class PingCommand: ICommand
    {
        public async Task Run(IContext e)
        {
            await e.Respond("pong!");
        }
    }
}
