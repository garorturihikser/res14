using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using Microsoft.CSharp.RuntimeBinder;
using TicTacToe;
using static TicTacToe.SquareState;
using static TicTacToe.GameState;

namespace DiscordBot.Commands
{
    public class TicTacToeCommand : ICommand
    {
        private TicTacToeManager Manager { get; set; }

        private Dictionary<char, SquareState> translationDict = new Dictionary<char, SquareState>
        {
            {'❌', X},
            {'⭕', O},
            {'⬜', Empty},
        };

        private string InvalidText = "Invalid screen";
        private string WinText = "You won!!";
        private string LoseText = "You lost :(";
        private string DrawText = "Game drawn";

        public TicTacToeCommand(TicTacToeManager manager)
        {
            Manager = manager;
        }

        public async Task Run(DiscordMessage msg)
        {
            var splitMsg = CommandParser.Split(msg.Content);

            switch (splitMsg[1])
            {
                case "start":
                    await msg.RespondAsync(StartText(msg));
                    break;

                case "stop":
                    await msg.RespondAsync(StopText(msg));
                    break;

                case "help":
                    await msg.RespondAsync(HelpText());
                    break;

                default:
                {
                    var game = Manager.GetGame(msg);

                    if (Manager.GameExists(game))
                    {
                        IEnumerable<string> textIterator = TurnText(msg, game);

                        foreach (string text in textIterator)
                        {
                            await msg.RespondAsync(text);
                        }
                    }

                    break;
                }
            }
        }

        private string HelpText() =>
            "Each turn the bot will send a tic tac toe screen." +
            " Place an :x: wherever you want to play and send the screen." +
            "\nMake sure to avoid spaces and other unnecessary SquareStateacters." +
            "\nFirst to complete a row, column, or diagonal wins.";

        /// <summary>
        /// The correct text to send after the user tried to start the game
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string StartText(DiscordMessage msg)
        {
            var started = Manager.AddGame(msg);

            // Game failed to start
            if (!started)
                return "Game already taking place";

            // Game started successfully
            else
            {
                TicTacToeGame game = Manager.GetGame(msg);
                char[,] screen = game.ScreenToMatrix(translationDict);

                return MatrixToString(screen);
            }
        }

        /// <summary>
        /// The correct text to send after the user tried to stop the game
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string StopText(DiscordMessage msg)
        {
            var stopped = Manager.RemoveGame(msg);
            return !stopped ? "No game currently taking place" : "Game stopped";
        }

        /// <summary>
        /// The correct text to send after the user sends a new screen (i.e. makes a move)
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        private IEnumerable<string> TurnText(DiscordMessage msg, TicTacToeGame game)
        {
            // Compare the new screen to the old screen
            // In order to see if any non-valid changes were made
            char[,] receivedScreen = StringToMatrix(msg.Content);

            GameState gameState = game.DoTurn(receivedScreen, translationDict);
            char[,] screenMatrix = game.ScreenToMatrix(translationDict);

            string finalScreenString = MatrixToString(screenMatrix);

            switch (gameState)
            {
                case InvalidScreen:
                    yield return InvalidText;
                    break;

                // In case the player won
                case XWin:
                    Manager.RemoveGame(msg);
                    yield return WinText;
                    break;

                // In case the computer won
                case OWin:
                    Manager.RemoveGame(msg);
                    yield return finalScreenString;
                    yield return LoseText;
                    break;
                
                case Draw:
                    Manager.RemoveGame(msg);
                    yield return DrawText;
                    break;
                
                // In case the game is continuing
                default:
                    yield return finalScreenString;
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
    }
}