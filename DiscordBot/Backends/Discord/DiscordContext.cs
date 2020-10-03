using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace DiscordBot.Backends.Discord
{
    public class DiscordContext: IContext
    {
        public MessageCreateEventArgs Msg { get; set; }

        public DiscordContext(MessageCreateEventArgs e)
        {
            Msg = e;
        }
        
        public string ExtractMessageContent()
        {
            return Msg.Message.Content;
        }

        public DiscordUser GetSender() =>
            Msg.Author;

        public DiscordChannel GetChannel() =>
            Msg.Channel;

        public async Task Respond(string content)
        {
            await Msg.Message.RespondAsync(content);
        }
    }
}