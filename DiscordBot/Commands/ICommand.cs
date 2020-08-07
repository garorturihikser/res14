using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DiscordBot.Commands
{
    // Commands that only send a message in response
    interface ICommand
    {
        Task Run(DiscordMessage msg);
    }
}
