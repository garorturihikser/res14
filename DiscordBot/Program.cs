using System.Collections.Generic;
using DiscordBot.Backends;
using DiscordBot.Backends.CLI;
using DiscordBot.Config;
using TicTacToe;
using botCommands = DiscordBot.Commands;

namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var discordTranslationDict = new Dictionary<char, SquareState>
            {
                {'❌' , SquareState.X},
                {'⭕', SquareState.O},
                {'⬜', SquareState.Empty},
            };

            var CLITranslationDict = new Dictionary<char, SquareState>
            {
                {'x', SquareState.X},
                {'o', SquareState.O},
                {'-', SquareState.Empty}
            };
            
            var ticTacToeCommand =
                new botCommands.TicTacToeCommand(new TicTacToeManager<char>(), CLITranslationDict);
            
            var commands = new Dictionary<string[], botCommands.ICommand>
            {
                { new []{"ping"}, new botCommands.PingCommand() },
                {new []{"tic", "tac", "toe"}, ticTacToeCommand},
            };
            
            var configGetter = new JsonConfigGetter("config.json");
            var config = configGetter.GetConfig();
            var commandProvider = new botCommands.CommandProvider(commands, config.Prefix);

            var bot = new Bot<string>(commandProvider, config);
            var backend = new CLIBackend(bot);

            bot.Run(backend).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
