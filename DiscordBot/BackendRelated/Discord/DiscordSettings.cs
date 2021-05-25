using System;
using System.Collections.Generic;
using DiscordBot.Config;
using DSharpPlus.EventArgs;
using TicTacToe;

namespace DiscordBot.BackendRelated.Discord
{
    public class DiscordSettings: IBackendSettings
    {
        public Dictionary<char, SquareState> TranslationDict { get; }

        public System.Type BackendType { get; }

        public System.Type Backend { get; }
        
        public System.Type ConfigType { get; }
        
        public string ConfigPath { get; set; }

        public DiscordSettings()
        {
            TranslationDict =  new Dictionary<char, SquareState>{
                {'❌' , SquareState.X},
                {'⭕', SquareState.O},
                {'⬜', SquareState.Empty},
            };

            BackendType = typeof(MessageCreateEventArgs);

            Backend = typeof(DiscordBackend);

            ConfigType = typeof(DiscordConfig);

            ConfigPath = "config.json";
        }
    }
}