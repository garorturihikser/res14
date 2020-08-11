using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

namespace TicTacToe
{
    public class TicTacToeGame
    {
        public char[,] Screen { get; set; }
        public char MySign { get; set; }
        public bool IsMyTurn { get; set; }

        public TicTacToeGame(char mySign)
        {
            // "n" - empty; "x" - x; "o" - o
            Screen = new char[,]{{'n', 'n', 'n'}, 
                {'n', 'n', 'n'},
                {'n', 'n', 'n'}};
            
            MySign = mySign;
            
            IsMyTurn = MySign == 'o';
        }

        /*public bool DoTurn(int[] move = null)
        {
            bool turnDone = false;
            
            if (IsDraw() && IsWin(MySign))
                return false;
                    
            if (IsMyTurn)
                turnDone = Move();

            else
            {
                // Make a move with the correct sign
                //var opSign = "xo".Replace(MySign, "");
                var opSign = 'x';
                turnDone = RegisterMove(move, opSign);
            }

            return turnDone;
        }*/

        public void UpdateScreen(char[,] screen) => 
            Screen = screen;
        
        public bool RegisterMove(int[] move, char sign)
        {
            if (!ValidMove(move))
                return false;
            
            Screen[move[0], move[1]] = sign;
            
            IsMyTurn = !IsMyTurn;
            
            return true;
        }

        public bool Move()
        {
            List<int[]> availableMoves = GetAvailableMoves();
            
            // Pick a random move and update the screen accordingly
            var random = new Random();
            var move = availableMoves[random.Next(availableMoves.Count)];
            
            var moveDone = RegisterMove(move, MySign);

            return moveDone;
        }
        
        /// <summary>
        /// Returns all the available moves
        /// </summary>
        /// <returns></returns>
        public List<int[]> GetAvailableMoves()
        {
            var availableMoves = new List<int[]>();
            
            // Populate the list
            for (int i = 0; i < Screen.GetLength(0); i++)
            {
                for (int j = 0; j < Screen.GetLength(1); j++)
                {
                    if (Screen[i, j] == 'n')
                    {
                        availableMoves.Add(new int[]{i, j});
                    }
                }
            }

            return availableMoves;
        }

        public bool IsDraw() => GetAvailableMoves().Count == 0;
        
        
        public bool IsWin(char sign)
        {
            bool[] rowWins = {true, true, true};
            bool[] columnWins = {true, true, true};
            bool[] diagonalWins = {true, true};
            
            for (int i = 0; i < Screen.GetLength(0); i++)
            {
                for (int j = 0; j < Screen.GetLength(1); j++)
                {
                    if (Screen[i, j] != sign)
                    {
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
            }

            foreach (bool win in rowWins)
            {
                Console.Write($"row win {win} ");
                if (win)
                    return win;
            }

            Console.WriteLine();
            
            foreach (bool win in columnWins)
            {
                Console.Write($"column win {win} ");
                if (win)
                    return win;
            }

            Console.WriteLine();
            
            foreach (bool win in diagonalWins)
            {
                Console.Write($"diagonal win {win} ");
                if (win)
                    return win;
            }
            
            return false;
        }

        public bool ValidMove(int[] move) =>
            Screen[move[0], move[1]] == 'n';
        
        
    }
}