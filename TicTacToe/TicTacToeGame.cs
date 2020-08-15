using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Microsoft.CSharp.RuntimeBinder;
using static TicTacToe.SquareState;
using static TicTacToe.GameState;

namespace TicTacToe
{
    public class TicTacToeGame
    {
        public SquareState[,] Screen { get; set; }
        // public SquareState MySign { get; set; }
        // public bool IsMyTurn { get; set; }

        public TicTacToeGame(SquareState mySign)
        {
            Screen = new SquareState[,]{{Empty, Empty, Empty}, 
                {Empty, Empty, Empty},
                {Empty, Empty, Empty}};
            
            // MySign = mySign;
            
            // IsMyTurn = MySign == O; - If I ever want to add O as a playable sign
        }
        
        /// <summary>
        /// Does one game turn
        /// </summary>
        /// <param name="screenMatrix">The user inputted screen</param>
        /// <param name="translationDict">A dictionary to translate the inputted screen into a game screen</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public GameState DoTurn<T>(T[,] screenMatrix, Dictionary<T, SquareState> translationDict)
        {
            SquareState[,] screen = MatrixToScreen(screenMatrix, translationDict);

            if (!IsScreenValid(screen, Screen)) 
                return InvalidScreen;

            UpdateScreen(screen);

            var draw = IsDraw();
            
            if (!draw)
                Move();

            switch (WinningSign())
            {
                case X:
                    return XWin;

                case O:
                    return OWin;

                default:
                    if (IsDraw())
                        return Draw;
                    break;
            }
            
            return draw ? Draw : GoingOn;
        }
    
        /// <summary>
        /// Updates the screen to a given screen
        /// </summary>
        /// <param name="screen"></param>
        private void UpdateScreen(SquareState[,] screen) => 
            Screen = screen;
        
        /// <summary>
        /// Displays the given move on the screen
        /// </summary>
        /// <param name="move"></param>
        /// <param name="sign"></param>
        private void RegisterMove(int[] move, SquareState sign) =>
            Screen[move[0], move[1]] = sign;
        
        /// <summary>
        /// Returns whether a move is valid and can be made
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        private bool ValidMove(int[] move) =>
            Screen[move[0], move[1]] == Empty;
        
        /// <summary>
        /// Makes a move
        /// </summary>
        private void Move()
        {
            List<int[]> availableMoves = GetAvailableMoves();
            
            // Pick a random move and update the screen accordingly
            var random = new Random();
            var move = availableMoves[random.Next(availableMoves.Count)];
            
            RegisterMove(move, O); // Change to MySign if I want ability to play as O
        }
        
        /// <summary>
        /// Returns all the available moves
        /// </summary>
        /// <returns></returns>
        private List<int[]> GetAvailableMoves()
        {
            var availableMoves = new List<int[]>();

            // Populate the list
            for (int i = 0; i < Screen.GetLength(0); i++)
            {
                for (int j = 0; j < Screen.GetLength(1); j++)
                {
                    var move = new[] {i, j};
                    if (ValidMove(move))
                    {
                        availableMoves.Add(move);
                    }
                }
            }

            return availableMoves;
        }
        
        /// <summary>
        /// Returns whether the game has resulted in a draw
        /// </summary>
        /// <returns></returns>
        private bool IsDraw() => GetAvailableMoves().Count == 0;
        
        /// <summary>
        /// Returns which sign won the game
        /// </summary>
        /// <returns>X, O, or Empty if no one won</returns>
        private SquareState WinningSign()
        {
            foreach (var state in new[] {X, O})
            {
                if (DidSignWin(state))
                    return state;
            }

            return Empty;
        }
        
        /// <summary>
        /// Checks whether a given sign won the game
        /// </summary>
        /// <param name="sign"></param>
        /// <returns></returns>
        private bool DidSignWin(SquareState sign)
        {
            bool[] rowWins = {true, true, true};
            bool[] columnWins = {true, true, true};
            bool[] diagonalWins = {true, true};
            
            for (int i = 0; i < Screen.GetLength(0); i++)
            {
                for (int j = 0; j < Screen.GetLength(1); j++)
                {
                    if (Screen[i, j] == sign) continue;
                    
                    rowWins[i] = false;
                    columnWins[j] = false;
                    
                    if (i == j)
                    {
                        diagonalWins[0] = false;
                        if (i == 1)
                            diagonalWins[1] = false;
                    }
                        
                    else if (Math.Abs(i - j) == Screen.GetLength(0) - 1)
                        diagonalWins[1] = false;
                }
            }

            return rowWins.Any(win => win)
                   || columnWins.Any(win => win)
                   || diagonalWins.Any(win => win);
        }
        
        private bool IsScreenValid(SquareState[,] newScreen, SquareState[,] oldScreen)
        {
            Dictionary<SquareState, SquareState> differences = GenerateScreenDifferences(newScreen, oldScreen);
            return AreDifferencesValid(differences);
        }
        
        /// <summary>
        /// Checks whether the differences between 2 screens are valid
        /// </summary>
        /// <param name="differences"></param>
        /// <returns></returns>
        private bool AreDifferencesValid(Dictionary<SquareState, SquareState> differences) =>
            differences.Count == 1
            && !differences.ContainsKey(O) // Change to MySign if I want ability to play as O
            && differences.Values.All(x => x == Empty);
        
        /// <summary>
        /// Generates and returns the differences between 2 given screens
        /// </summary>
        /// <param name="newScreen"></param>
        /// <param name="oldScreen"></param>
        /// <returns>Dictionary of SquareStates representing the same square index in both screens</returns>
        private Dictionary<SquareState, SquareState> GenerateScreenDifferences(SquareState[,] newScreen, SquareState[,] oldScreen)
        {
            var differences = new Dictionary<SquareState, SquareState>();

            for (int i = 0; i < newScreen.GetLength(0); i++)
            {
                for (int j = 0; j < oldScreen.GetLength(1); j++)
                {
                    SquareState newSquareState = newScreen[i, j];
                    SquareState oldSquareState = oldScreen[i, j];

                    if (newSquareState != oldSquareState)
                        differences.Add(newSquareState, oldSquareState);
                }
            }

            return differences;
        }
        
        public T[,] ScreenToMatrix<T>(Dictionary<T, SquareState> translationDict)
        {
            var screen = new T[Screen.GetLength(0), Screen.GetLength(1)];

            for (int i = 0; i < Screen.GetLength(0); i++)
            {
                for (int j = 0; j < Screen.GetLength(1); j++)
                {
                    screen[i, j] = translationDict.Keys.Where(x => translationDict[x] == Screen[i, j]).ToArray()[0];
                }
            }

            return screen;
        }
        
        private SquareState[,] MatrixToScreen<T>(T[,] screenMatrix, Dictionary<T, SquareState> translationDict)
        {
            var screen = new SquareState[screenMatrix.GetLength(0), screenMatrix.GetLength(1)];

            for (int i = 0; i < screenMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < screenMatrix.GetLength(1); j++)
                {
                    screen[i, j] = translationDict[screenMatrix[i, j]];
                }
            }

            return screen;
        }
    }
}