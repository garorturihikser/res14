using System.Collections.Generic;
using DSharpPlus.EventArgs;
using TicTacToe;

namespace DiscordBot.Backends.Discord
{
    public class DiscordSettings
    {
        public Dictionary<char, SquareState> TranslationDict { get; }

        public System.Type BackendType { get; }

        public System.Type Backend { get; }

        public DiscordSettings()
        {
            TranslationDict =  new Dictionary<char, SquareState>{
                {'❌' , SquareState.X},
                {'⭕', SquareState.O},
                {'⬜', SquareState.Empty},
            };

            BackendType = typeof(MessageCreateEventArgs);

            Backend = typeof(DiscordBackend);
        }
    }
}