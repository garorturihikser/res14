using System.Collections.Generic;
using DiscordBot.Config;
using Tweetinvi;
using TicTacToe;
using Tweetinvi.Models;

namespace DiscordBot.BackendRelated.Twitter
{
    public class TwitterSettings: IBackendSettings
    {
        public Dictionary<char, SquareState> TranslationDict { get; }

        public System.Type BackendType { get; }

        public System.Type Backend { get; }

        public System.Type ConfigType { get; }
        
        public string ConfigPath { get; set; }

        public TwitterSettings()
        {
            TranslationDict =  new Dictionary<char, SquareState>{
                {'❌' , SquareState.X},
                {'⭕', SquareState.O},
                {'⬜', SquareState.Empty},
            };
            
            // Put the twitter message button here.
            // For now i think only make the bot in private messages
            BackendType = typeof(IMessage);

            Backend = typeof(TwitterBackend);

            ConfigType = typeof(TwitterSettings);

            ConfigPath = "twitter-config.json";
        }
    }
}