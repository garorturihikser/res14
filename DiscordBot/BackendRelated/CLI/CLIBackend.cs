using System;
using System.Threading.Tasks;

namespace DiscordBot.BackendRelated.CLI
{
    class CLIBackend: IBackend<string>
    {
        private Func<IContext, Task> _botMessageHandler;

        public async Task Run(Func<IContext, Task> botMessageHandler)
        {
            _botMessageHandler = botMessageHandler;
            while (true)
            {
                var text = Console.ReadLine();
                await MessageHandler(text);
            }
        }

        public async Task MessageHandler(string e)
        {
             var context = new CLIContext(e);
             await _botMessageHandler(context);
        }
    }
}