using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBot.BackendRelated;
using TicTacToe;

namespace DiscordBot.CommandRelated.Commands
{
    class TicTacToeCommand: ICommand
    {
        private TicTacToeManager<char> Manager { get; set; }
        private Dictionary<char, SquareState> TranslationDict;

        private Dictionary<GameState, string> _responses = new Dictionary<GameState, string>()
        {
            {GameState.InvalidScreen, "Invalid screen"},
            {GameState.XWin, "You won!!"},
            {GameState.OWin, "You lost :("},
            {GameState.Draw, "Game drawn"}
        };

        public TicTacToeCommand(TicTacToeManager<char> manager, Dictionary<char, SquareState> translationDict)
        {
            Manager = manager;
            TranslationDict = translationDict;
        }

        private async Task HandleTicTacToeScreen(IContext e)
        {
            var game = Manager.GetGame(e);

            if (Manager.GameExists(game))
            {
                await RunTurnSubcommand(e, game);
            } 
        }

        public async Task OnRegularMessage(IContext e)
        {
            if (IsTicTacToeScreen(e.ExtractMessageContent()))
            {
                await HandleTicTacToeScreen(e);
            }
        }

        public async Task Run(IContext msg)
        {
            var splitMsg = CommandParser.Split(msg.ExtractMessageContent());

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

        private async Task HelpText(IContext msg)
        {
            var text = "Each turn the bot will send a tic tac toe screen." +
                       " Place an :x: wherever you want to play and send the screen." +
                       "\nMake sure to avoid spaces and other unnecessary characters." +
                       "\nFirst to complete a row, column, or diagonal wins.";

            await msg.Respond(text);
        }

        /// <summary>
        /// Attempts to start a game, and sends the user a message accordingly.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private async Task RunStartSubcommand(IContext msg)
        {
            var started = Manager.AddGame(msg, TranslationDict);
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
            
            await msg.Respond(text);
        }
        
        /// <summary>
        /// Attempts to stop the game, and sends the user a message accordingly.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private async Task RunStopSubcommand(IContext msg)
        {
            var stopped = Manager.RemoveGame(msg);
            var text = !stopped ? "No game currently taking place" : "Game stopped";

            await msg.Respond(text);
        }

        /// <summary>
        /// The correct text to send after the user sends a new screen (i.e. makes a move)
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        private async Task RunTurnSubcommand(IContext msg, TicTacToeGame<char> game)
        {
            // Compare the new screen to the old screen
            // In order to see if any non-valid changes were made
            char[,] receivedScreen = StringToMatrix(msg.ExtractMessageContent());

            GameState gameState = game.DoTurn(receivedScreen);
            char[,] screenMatrix = game.ScreenToMatrix();

            string finalScreenString = MatrixToString(screenMatrix);

            switch (gameState)
            {
                // In case the game is continuing
                case GameState.GoingOn:
                    await msg.Respond(finalScreenString);
                    return;
                
                // In case the player won
                case GameState.XWin:
                    Manager.RemoveGame(msg);
                    break;

                // In case the computer won
                case GameState.OWin:
                    Manager.RemoveGame(msg);
                    await msg.Respond(finalScreenString);
                    break;
                
                case GameState.Draw:
                    Manager.RemoveGame(msg);
                    break;
            }

            await msg.Respond(_responses[gameState]);
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
            var squareNum = ticTacToeScreenHeight * ticTacToeScreenWidth;
            foreach (char chr in content)
            {
                if (TranslationDict.Keys.All(x => chr != x && chr != '\n'))
                    return false;
            }

            return squareNum + numOfNewLines == content.Length || squareNum == content.Length;
        }
    }
}