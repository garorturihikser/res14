using System;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;

namespace DiscordBot.Backends.Discord
{
    class DiscordBackend: IBackend<MessageCreateEventArgs>
    {
        readonly Bot<MessageCreateEventArgs> _bot;

        public DiscordBackend(Bot<MessageCreateEventArgs> bot) =>
            _bot = bot;

        public async Task Run()
        {
            _bot.DiscordClient.MessageCreated += MessageHandler;

            await _bot.DiscordClient.ConnectAsync();
            await Task.Delay(-1);
        }

        private async Task MessageHandler(MessageCreateEventArgs e)
        {
            var context = new DiscordContext(e);
            await _bot.MessageHandler(context);
        }
        
    }
}