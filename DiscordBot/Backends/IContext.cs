using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DiscordBot.Backends
{
    public interface IContext
    {
        public string ExtractMessageContent();
        public DiscordUser GetSender();
        public DiscordChannel GetChannel();
        public Task Respond(string content);
    }
}