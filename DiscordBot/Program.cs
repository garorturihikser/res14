using System;
using System.Collections.Generic;
using DiscordBot.Backends;
using DiscordBot.Backends.CLI;
using DiscordBot.Backends.Discord;
using DiscordBot.Config;
using DSharpPlus.EventArgs;
using TicTacToe;
using botCommands = DiscordBot.Commands;

namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new DiscordSettings();

            var CLITranslationDict = new Dictionary<char, SquareState>
            {
                {'x', SquareState.X},
                {'o', SquareState.O},
                {'-', SquareState.Empty}
            };
            
            var ticTacToeCommand =
                new botCommands.TicTacToeCommand(new TicTacToeManager<char>(), settings.TranslationDict);
            
            var commands = new Dictionary<string[], botCommands.ICommand>
            {
                { new []{"ping"}, new botCommands.PingCommand()},
                {new []{"tic", "tac", "toe"}, ticTacToeCommand}
            };
            
            var configGetter = new JsonConfigGetter("config.json");
            var config = configGetter.GetConfig();
            var commandProvider = new botCommands.CommandProvider(commands, config.Prefix);
            
            var type = typeof(Bot<>).MakeGenericType(settings.BackendType);
            dynamic bot = Activator.CreateInstance(type, commandProvider, config);
            
            var backend = Activator.CreateInstance(settings.Backend, bot);

            bot.Run(backend).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
