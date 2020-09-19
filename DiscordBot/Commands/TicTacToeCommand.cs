using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Extensions.DependencyInjection;
using TicTacToe;

namespace DiscordBot.Commands
{
    public class TicTacToeCommand : ICommand
    {
        private TicTacToeManager<char> Manager { get; set; }
        
        private Dictionary<char, SquareState> translationDict = new Dictionary<char, SquareState>
        {
            {'❌', SquareState.X},
            {'⭕', SquareState.O},
            {'⬜', SquareState.Empty},
        };

        private string InvalidText = "Invalid screen";
        private string WinText = "You won!!";
        private string LoseText = "You lost :(";
        private string DrawText = "Game drawn";

        public TicTacToeCommand(TicTacToeManager<char> manager)
        {
            Manager = manager;
        }

        private async Task HandleTicTacToeScreen(DiscordMessage msg)
        {
            var game = Manager.GetGame(msg);

            if (Manager.GameExists(game))
            {
                await RunTurnSubcommand(msg, game);
            } 
        }

        public async Task OnMessage(MessageCreateEventArgs e)
        {
            if (IsTicTacToeScreen(e.Message.Content))
            {
                await HandleTicTacToeScreen(e.Message);
            }
        }

        public async Task Run(DiscordMessage msg)
        {
            var splitMsg = CommandParser.Split(msg.Content);

            switch (splitMsg[1])
            {
                case "start":
                    await RunStartSubcommand(msg);
                    break;

                case "stop":
                    await RunStopSubcommand(msg);
                    break;

                case "help":
                    await HelpText(msg);
                    break;
            }
        }

        private async Task HelpText(DiscordMessage msg)
        {
            var text = "Each turn the bot will send a tic tac toe screen." +
                       " Place an :x: wherever you want to play and send the screen." +
                       "\nMake sure to avoid spaces and other unnecessary characters." +
                       "\nFirst to complete a row, column, or diagonal wins.";

            await msg.RespondAsync(text);
        }

        /// <summary>
        /// Attempts to start a game, and sends the user a message accordingly.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private async Task RunStartSubcommand(DiscordMessage msg)
        {
            var started = Manager.AddGame(msg, translationDict);
            var text = "";
            
            // Game failed to start
            if (!started)
                text =  "Game already taking place";

            // Game started successfully
            else
            {
                var game = Manager.GetGame(msg);
                char[,] screen = game.ScreenToMatrix();

                text = MatrixToString(screen);
            }
            
            await msg.RespondAsync(text);
        }
        
        /// <summary>
        /// Attempts to stop the game, and sends the user a message accordingly.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private async Task RunStopSubcommand(DiscordMessage msg)
        {
            var stopped = Manager.RemoveGame(msg);
            var text = !stopped ? "No game currently taking place" : "Game stopped";

            await msg.RespondAsync(text);
        }

        /// <summary>
        /// The correct text to send after the user sends a new screen (i.e. makes a move)
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        private async Task RunTurnSubcommand(DiscordMessage msg, TicTacToeGame<char> game)
        {
            // Compare the new screen to the old screen
            // In order to see if any non-valid changes were made
            char[,] receivedScreen = StringToMatrix(msg.Content);

            GameState gameState = game.DoTurn(receivedScreen);
            char[,] screenMatrix = game.ScreenToMatrix();

            string finalScreenString = MatrixToString(screenMatrix);

            switch (gameState)
            {
                case GameState.InvalidScreen:
                    await msg.RespondAsync(InvalidText);
                    break;

                // In case the player won
                case GameState.XWin:
                    Manager.RemoveGame(msg);
                    await msg.RespondAsync(WinText);
                    break;

                // In case the computer won
                case GameState.OWin:
                    Manager.RemoveGame(msg);
                    await msg.RespondAsync(finalScreenString);
                    await msg.RespondAsync(LoseText);
                    break;
                
                case GameState.Draw:
                    Manager.RemoveGame(msg);
                    await msg.RespondAsync(DrawText);
                    break;
                
                // In case the game is continuing
                default:
                    await msg.RespondAsync(finalScreenString);
                    break;
            }
        }

        private string MatrixToString(char[,] matrix)
        {
            var str = "";
            int counter = 0;

            foreach (char chr in matrix)
            {
                str += chr;
                counter++;
                
                if (counter % 3 == 0)
                    str += "\n";
            }

            return str;
        }

        private char[,] StringToMatrix(string str)
        {
            str = str.Replace("\n", "");

            var matrix = new char[3, 3];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = str[i * 3 + j];
                }
            }

            return matrix;
        }
        
        private bool IsTicTacToeScreen(string content)
        {
            var ticTacToeScreenHeight = 3;
            var ticTacToeScreenWidth = 3;
            var numOfNewLines = 2;

            foreach (char chr in content)
            {
                if (translationDict.Keys.All(x => chr != x && chr != '\n'))
                    return false;
            }

            return ticTacToeScreenHeight * ticTacToeScreenWidth + numOfNewLines == content.Length;
        }
    }
}