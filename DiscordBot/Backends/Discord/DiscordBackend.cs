using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace DiscordBot.Backends.Discord
{
    class DiscordBackend: IBackend<MessageCreateEventArgs>
    {
        readonly DiscordClient _client;
        
        readonly Bot<MessageCreateEventArgs> _bot;

        public DiscordBackend(string token, Bot<MessageCreateEventArgs> bot)
        {
            _client = new DiscordClient(new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot
            });

            _bot = bot;
        }

        public async Task Run()
        {
            _client.MessageCreated += MessageHandler;

            await _client.ConnectAsync();
            await Task.Delay(-1);
        }

        private async Task MessageHandler(MessageCreateEventArgs e)
        {
            var context = new DiscordContext(e);
            await _bot.MessageHandler(context);
        }
        
    }
}