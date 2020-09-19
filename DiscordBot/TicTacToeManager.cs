using System;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.Entities;
using TicTacToe;

namespace DiscordBot
{
    public class TicTacToeManager<T>
    {
        private Dictionary<Tuple<DiscordUser, DiscordChannel>, TicTacToeGame<T>> Players { get; set; }

        public TicTacToeManager() =>
            Players = new Dictionary<Tuple<DiscordUser, DiscordChannel>, TicTacToeGame<T>>();

        public bool AddGame(DiscordMessage msg, Dictionary<T, SquareState> translationDict)
        {
            var gameExists = GameExists(GetGame(msg));
            
            if (!gameExists)
                Players.Add(TupleFromMessage(msg), new TicTacToeGame<T>(translationDict));

            return !gameExists;
        }

        public bool RemoveGame(DiscordMessage msg)
        {
            var gameExists = GameExists(GetGame(msg));
            
            if (gameExists)
                Players.Remove(TupleFromMessage(msg));

            return gameExists;
        }
        
        public TicTacToeGame<T> GetGame(DiscordMessage msg)
        {
            Players.TryGetValue(TupleFromMessage(msg), out var game);
            return game;
        }

        public bool GameExists(TicTacToeGame<T> game) =>
            game != null;
        
        private Tuple<DiscordUser, DiscordChannel> TupleFromMessage(DiscordMessage msg) =>
            new Tuple<DiscordUser, DiscordChannel>(msg.Author, msg.Channel);
    }
}