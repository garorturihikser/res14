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
            new TicTacToeCommand(new TicTacToeManager());
        
        static Dictionary<string, ICommand> Commands = new Dictionary<string, ICommand>
        {
            { "ping", new PingCommand() },
            {"tic", _ticTacToeCommand},
            {"tac", _ticTacToeCommand},
            {"toe", _ticTacToeCommand},
        };
        
        static void Main(string[] args)
        {
            // fuck tom.... <3
            var path = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())));
            var configGetter = new JsonConfigGetter(path + $"\\config.json");
            var config = configGetter.GetConfig();
            var commandGetter = new CommandGetter(Commands, config.Prefix);

            var bot = new Bot(commandGetter, config);
            
            bot.Run().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
