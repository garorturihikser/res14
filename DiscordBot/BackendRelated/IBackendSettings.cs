using System.Collections.Generic;
using DSharpPlus.EventArgs;
using TicTacToe;

namespace DiscordBot
{
    public interface IBackendSettings
    {
        Dictionary<char, SquareState> TranslationDict { get; }

        System.Type BackendType { get; }

        System.Type Backend { get; }
        
        System.Type ConfigType { get; }
        
        string ConfigPath { get; }
    }
}