using System.Threading.Tasks;
using DiscordBot.Backends;

namespace DiscordBot.Commands
{
    // Commands that only send a message in response
    public interface ICommand
    {
        Task Run(IContext e);

        Task OnMessage(IContext e)
        {
            return Task.CompletedTask;
        }
    }
}
