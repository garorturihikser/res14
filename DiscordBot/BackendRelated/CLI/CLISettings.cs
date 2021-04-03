using System;
using System.Collections.Generic;
using DSharpPlus.EventArgs;
using TicTacToe;

namespace DiscordBot.BackendRelated.CLI
{
    public class CLISettings: IBackendSettings
    {
        public Dictionary<char, SquareState> TranslationDict { get; }

        public System.Type BackendType { get; }

        public System.Type Backend { get; }
        
        public System.Type ConfigType { get; }

        public string ConfigPath { get; }

        public CLISettings()
        {
            TranslationDict =  new Dictionary<char, SquareState>{
                {'x' , SquareState.X},
                {'o', SquareState.O},
                {'-', SquareState.Empty},
            };

            BackendType = typeof(string);

            Backend = typeof(CLIBackend);

            ConfigType = typeof(Nullable);

            ConfigPath = "config.json";
        }
    }
}