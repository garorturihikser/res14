namespace DiscordBot.Config
{
    public class TwitterConfig: IConfig
    {
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
        public string Prefix { get; set; }
    }
}