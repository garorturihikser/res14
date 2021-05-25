using System.Threading.Tasks;
using DiscordBot.BackendRelated;

namespace DiscordBot.CommandRelated.Commands
{
    class PingCommand: ICommand
    {
        public async Task Run(IContext e)
        {
            await e.Respond("pong!");
        }
    }
}
