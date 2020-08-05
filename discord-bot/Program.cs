using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using discord_bot.Commands;
using discord_bot.Configurations;
using DSharpPlus;
using Newtonsoft.Json;

namespace discord_bot
{
    class Program
    {
        private static DiscordClient discord;
        private static string commandPrefix;
        private static string token;
        private static Dictionary<string, ICommand> commandDict = new Dictionary<string, ICommand>();

        static void Main(string[] args)
        {
            SetConfigurations();
            SetCommands();
            ISortCommand commandSorter = new SortCommand();
            MainAsync(args, commandSorter).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Sets the configurations for the bot
        /// </summary>
        static void SetConfigurations()
        {
            string wanted_path = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())));
            System.IO.StreamReader r = new StreamReader($"{wanted_path}\\Configurations\\config.json");

            string json = r.ReadToEnd();
            Config configs = JsonConvert.DeserializeObject<Config>(json);

            token = configs.Token;
            commandPrefix = configs.Prefix;
        }

        /// <summary>
        /// Sets the commands the bot will respond to
        /// </summary>
        static void SetCommands()
        {
            commandDict.Add("ping", new Ping());
        }

        /// <summary>
        /// Receives messages and handles them
        /// </summary>
        /// <param name="args"></param>
        /// <param name="commandSorter">Forwards the commands to the correct classes</param>
        /// <returns></returns>
        static async Task MainAsync(string[] args, ISortCommand commandSorter)
        {
            // Instantiate the bot
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot
            });

            // We received a message
            discord.MessageCreated += async e =>
            {
                if (IsCommand(e))
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
        static bool IsCommand(DSharpPlus.EventArgs.MessageCreateEventArgs e)
        {
            return e.Message.Content.ToLower().StartsWith(commandPrefix);
        }
    }
}
