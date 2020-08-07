using System.IO;
using Newtonsoft.Json;

namespace DiscordBot.Config
{
    class JsonConfigGetter : IConfigGetter
    {
        private string Path { get; set; }
        public JsonConfigGetter(string path)
        {
            Path = path;
        }
        
        /// <summary>
        /// Returns the configurations for the bot
        /// </summary>
        public Config GetConfig()
        {
            
            using (StreamReader r = new StreamReader(Path))
            {
                string json = r.ReadToEnd();
                var config = JsonConvert.DeserializeObject<Config>(json);
                return config;
            }
        }
        

    }
}
