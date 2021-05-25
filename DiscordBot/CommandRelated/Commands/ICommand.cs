using System.Threading.Tasks;
using DiscordBot.BackendRelated;

namespace DiscordBot.CommandRelated.Commands
{
    // Commands that only send a message in response
    public interface ICommand
    {
        Task Run(IContext e);

        Task OnRegularMessage(IContext e)
        {
            return Task.CompletedTask;
        }
    }
}
