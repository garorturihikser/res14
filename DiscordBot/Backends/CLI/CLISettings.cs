using System.Collections.Generic;
using DiscordBot.Backends.Discord;
using DSharpPlus.EventArgs;
using TicTacToe;

namespace DiscordBot.Backends.CLI
{
    public class CLISettings
    {
        public Dictionary<char, SquareState> TranslationDict { get; }

        public System.Type BackendType { get; }

        public System.Type Backend { get; }

        public CLISettings()
        {
            TranslationDict =  new Dictionary<char, SquareState>{
                {'❌' , SquareState.X},
                {'⭕', SquareState.O},
                {'⬜', SquareState.Empty},
            };

            BackendType = typeof(string);

            Backend = typeof(CLIBackend);
        }
    }
}