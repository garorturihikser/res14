using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DiscordBot.BackendRelated.Discord;
using DiscordBot.BackendRelated.Twitter;
using DiscordBot.BackendRelated.CLI;
using DiscordBot.Config;
using DSharpPlus.EventArgs;
using TicTacToe;
using Tweetinvi.Core.Extensions;
using commandHandler = DiscordBot.CommandRelated;
using botCommands = DiscordBot.CommandRelated.Commands;

namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args)
        {
            // Discord v CLI
            var settings = new CLISettings();
            
            
            var ticTacToeCommand =
                new botCommands.TicTacToeCommand(new TicTacToeManager<char>(), settings.TranslationDict);
            
            var commands = new Dictionary<string[], botCommands.ICommand>
            {
                { new []{"ping"}, new botCommands.PingCommand()},
                {new []{"tic", "tac", "toe"}, ticTacToeCommand}
            };
            
            var configGetter = new JsonConfigGetter(settings.ConfigPath, settings.ConfigType);
            var config = configGetter.GetConfig();
            var commandProvider = new commandHandler.CommandProvider(commands, config.Prefix);
            
            object backend;
            
            // TODO - CHANGE THIS HORRIBLE THING
            try
            {
                backend = Activator.CreateInstance(settings.Backend, config);
            }
            catch (Exception e)
            {
                backend = Activator.CreateInstance(settings.Backend);
            }
            
            var botType = typeof(Bot<>).MakeGenericType(settings.BackendType);
            dynamic bot = Activator.CreateInstance(botType, commandProvider, backend, config);
            
            bot.Run().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
