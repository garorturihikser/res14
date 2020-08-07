using System.Collections.Generic;
using DiscordBot.Commands;
using DiscordBot.Config;

namespace DiscordBot
{
    class Program
    {
        static Dictionary<string, ICommand> Commands = new Dictionary<string, ICommand>
        {
            { "ping", new Ping() }
        };
        
        static void Main(string[] args)
        {
            // fuck tom.... <3
            var configGetter = new JsonConfigGetter();
            var config = configGetter.GetConfig();
            var commandGetter = new CommandGetter(Commands, config.Prefix);
            
            var bot = new Bot(commandGetter, config);
            
            bot.Run().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
