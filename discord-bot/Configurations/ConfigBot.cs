using discord_bot.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace discord_bot.Configurations
{
    static class ConfigBot
    {
        /// <summary>
        /// Returns the configurations for the bot
        /// </summary>
        public static Tuple<string, string> GetConfigurations()
        {
            string wanted_path = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())));
            System.IO.StreamReader r = new StreamReader($"{wanted_path}\\Configurations\\config.json");

            string json = r.ReadToEnd();
            Config configs = JsonConvert.DeserializeObject<Config>(json);

            return Tuple.Create(configs.Token, configs.Prefix);
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
