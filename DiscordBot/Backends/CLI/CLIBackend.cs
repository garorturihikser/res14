using System;
using System.Threading.Tasks;

namespace DiscordBot.Backends.CLI
{
    class CLIBackend: IBackend<string>
    {
        readonly Bot<string> _bot;

        public CLIBackend(Bot<string> bot)
        {
            _bot = bot;
        }

        public async Task Run()
        {
            while (true)
            {
                var text = Console.ReadLine();
                await MessageHandler(text);
            }
        }

        private async Task MessageHandler(string e)
        {
            var context = new CLIContext(e);
            await _bot.MessageHandler(context);
        }
    }
}