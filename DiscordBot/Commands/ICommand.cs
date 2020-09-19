using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace DiscordBot.Commands
{
    // Commands that only send a message in response
    public interface ICommand
    {
        Task Run(DiscordMessage msg);

        Task OnMessage(MessageCreateEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
