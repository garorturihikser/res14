using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DiscordBot.BackendRelated
{
    public interface IContext
    {
        string ExtractMessageContent();
        DiscordUser GetSender();
        DiscordChannel GetChannel();
        Task Respond(string content);
    }
}