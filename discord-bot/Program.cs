using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using discord_bot.Commands;
using discord_bot.Configurations;
using DSharpPlus;

namespace discord_bot
{
    class Program
    {

        static void Main(string[] args)
        {
            ISortCommand commandSorter = new SortCommand();
            MainAsync(args, commandSorter, ConfigBot.GetConfigurations()).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Receives messages and handles them
        /// </summary>
        /// <param name="args"></param>
        /// <param name="commandSorter">Forwards the commands to the correct classes</param>
        /// <returns></returns>
        static async Task MainAsync(string[] args, ISortCommand commandSorter, Tuple<string, string> configs)
        {
            string token = configs.Item1;
            string commandPrefix = configs.Item2;

            Dictionary<string, ICommand> commandDict = ConfigBot.GetCommands();

            // Instantiate the bot
            var discord = new DiscordClient(new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot
            });

            // We received a message
            discord.MessageCreated += async e =>
            {
                if (IsCommand(e, commandPrefix))
                {
                    Console.WriteLine(e.Message.Content);
                    await commandSorter.Sort(e.Message, commandDict, commandPrefix);
                }

            };
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        /// <summary>
        /// Checks whether a message is a command (starts with the command prefix)
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        static bool IsCommand(DSharpPlus.EventArgs.MessageCreateEventArgs e, string commandPrefix)
        {
            return e.Message.Content.ToLower().StartsWith(commandPrefix);
        }
    }
}
