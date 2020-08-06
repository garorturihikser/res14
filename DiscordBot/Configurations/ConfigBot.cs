using System;
using System.Collections.Generic;
using System.IO;
using DiscordBot.Commands;
using Newtonsoft.Json;

namespace DiscordBot.Configurations
{
    static class ConfigBot
    {
        /// <summary>
        /// Returns the configurations for the bot
        /// </summary>
        public static Config GetConfigurations()
        {
            string wantedPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())));
            System.IO.StreamReader r = new StreamReader($"{wantedPath}\\Configurations\\config.json");

            string json = r.ReadToEnd();
            Config configs = JsonConvert.DeserializeObject<Config>(json);

            return configs;
        }

        /// <summary>
        /// Sets the commands the bot will respond to
        /// </summary>
        public static Dictionary<string, ICommand> GetCommands()
        {
            Dictionary<string, ICommand> commandDict = new Dictionary<string, ICommand>
            {
                { "ping", new Ping() }
            };

            return commandDict;
        }
    }
}
