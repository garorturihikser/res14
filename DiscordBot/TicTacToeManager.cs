using System;
using System.Collections.Generic;
using DiscordBot.BackendRelated;
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

        public bool AddGame(IContext msg, Dictionary<T, SquareState> translationDict)
        {
            var gameExists = GameExists(GetGame(msg));
            
            if (!gameExists)
                Players.Add(TupleFromMessage(msg), new TicTacToeGame<T>(translationDict));

            return !gameExists;
        }

        public bool RemoveGame(IContext msg)
        {
            var gameExists = GameExists(GetGame(msg));
            
            if (gameExists)
                Players.Remove(TupleFromMessage(msg));

            return gameExists;
        }
        
        public TicTacToeGame<T> GetGame(IContext msg)
        {
            Players.TryGetValue(TupleFromMessage(msg), out var game);
            return game;
        }

        public bool GameExists(TicTacToeGame<T> game) =>
            game != null;
        
        private Tuple<DiscordUser, DiscordChannel> TupleFromMessage(IContext msg) =>
            new Tuple<DiscordUser, DiscordChannel>(msg.GetSender(), msg.GetChannel());
    }
}