using System;
using System.IO;
using Newtonsoft.Json;

namespace DiscordBot.Config
{
    class JsonConfigGetter : IConfigGetter
    {
        private string Path { get; set; }
        private System.Type ConfigType { get; set; }
        public JsonConfigGetter(string path, System.Type configType)
        {
            Path = path;
            ConfigType = configType;
        }
        
        /// <summary>
        /// Returns the configurations for the bot
        /// </summary>
        public IConfig GetConfig()
        {
            using StreamReader r = new StreamReader(Path);
            string json = r.ReadToEnd();
            
            IConfig config = null;
            if (ConfigType == typeof(DiscordConfig))
                config = JsonConvert.DeserializeObject<DiscordConfig>(json);
            else if (ConfigType == typeof(TwitterConfig))
                config = JsonConvert.DeserializeObject<TwitterConfig>(json);
            
            return config;
        }
        

    }
}
