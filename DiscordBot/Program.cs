using System;
using System.Collections.Generic;
using System.IO;
using DiscordBot.Commands;
using DiscordBot.Config;

namespace DiscordBot
{
    class Program
    {
        private static TicTacToeCommand _ticTacToeCommand =
            new TicTacToeCommand(new TicTacToeManager<char>());
        
        static Dictionary<string[], ICommand> Commands = new Dictionary<string[], ICommand>
        {
            { new []{"ping"}, new PingCommand() },
            {new []{"tic", "tac", "toe"}, _ticTacToeCommand},
        };
        
        static void Main(string[] args)
        {
            // fuck tom!!!!!!!!
            // heck tom
            // frick tom
            var configGetter = new JsonConfigGetter("config.json");
            var config = configGetter.GetConfig();
            var commandGetter = new CommandProvider(Commands, config.Prefix);

            var bot = new Bot(commandGetter, config);
            
            bot.Run().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
