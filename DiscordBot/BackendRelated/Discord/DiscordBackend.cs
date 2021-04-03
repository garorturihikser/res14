using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace DiscordBot.BackendRelated.Discord
{
    class DiscordBackend: IBackend<MessageCreateEventArgs>
    {
        private DiscordClient _client;
        private Func<IContext, Task> _botMessageHandler;

        // TODO - find a way for the bot to get the command prefix, without needing to send them both the config
        public DiscordBackend(Config.DiscordConfig discordConfig)
        {
            // Instantiate the client
            _client = new DiscordClient(new DiscordConfiguration
            {
                Token = discordConfig.Token,
                TokenType = TokenType.Bot
            });
        }
        
        public async Task Run(Func<IContext, Task> botMessageHandler)
        {
            // TODO - fix the very sloppy solution
            _botMessageHandler = botMessageHandler;
            
            _client.MessageCreated += MessageHandler;
            
            await _client.ConnectAsync();
            await Task.Delay(-1);
        }
        
        public async Task MessageHandler(MessageCreateEventArgs e)
        {
            var context = new DiscordContext(e);
            await _botMessageHandler(context);
        }
        
    }
}