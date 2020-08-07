using System.IO;
using Newtonsoft.Json;

namespace DiscordBot.Config
{
    class JsonConfigGetter : IConfigGetter
    {
        /// <summary>
        /// Returns the configurations for the bot
        /// </summary>
        public DiscordBot.Config.Config GetConfig()
        {
            string wantedPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())));
            System.IO.StreamReader r = new StreamReader($"{wantedPath}\\Configurations\\config.json");

            string json = r.ReadToEnd();
            DiscordBot.Config.Config configs = JsonConvert.DeserializeObject<DiscordBot.Config.Config>(json);

            return configs;
        }
        

    }
}
