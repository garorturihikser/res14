using System;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.Entities;
using TicTacToe;

namespace DiscordBot
{
    public class TicTacToeManager
    {
        private Dictionary<Tuple<DiscordUser, DiscordChannel>, TicTacToeGame> Players { get; set; }

        public TicTacToeManager() =>
            Players = new Dictionary<Tuple<DiscordUser, DiscordChannel>, TicTacToeGame>();

        public bool AddGame(DiscordMessage msg)
        {
            var gameExists = GameExists(GetGame(msg));
            
            if (!gameExists)
                Players.Add(TupleFromMessage(msg), new TicTacToeGame('o'));

            return !gameExists;
        }


        public bool RemoveGame(DiscordMessage msg)
        {
            var gameExists = GameExists(GetGame(msg));
            
            if (gameExists)
                Players.Remove(TupleFromMessage(msg));

            return gameExists;
        }
            

        public TicTacToeGame GetGame(DiscordMessage msg)
        {
            Players.TryGetValue(TupleFromMessage(msg), out var game);
            return game;
        }

        public bool GameExists(TicTacToeGame game) =>
            game != null;
        
        private Tuple<DiscordUser, DiscordChannel> TupleFromMessage(DiscordMessage msg) =>
            new Tuple<DiscordUser, DiscordChannel>(msg.Author, msg.Channel);
    }
}