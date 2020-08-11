using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.VisualBasic;
using TicTacToe;

namespace DiscordBot.Commands
{
    public class TicTacToeCommand : ICommand
    {
        private TicTacToeManager Manager { get; set; }

        private Dictionary<char, char> ScreenTranslator = new Dictionary<char, char>
        {
            {'❌', 'x'},
            {'⭕', 'o'},
            {'⬜', 'n'},
            {'x', '❌'}, 
            {'o', '⭕'}, 
            {'n', '⬜'}
        };
        
        public TicTacToeCommand(TicTacToeManager manager)
        {
            Manager = manager;
        }

        public async Task Run(DiscordMessage msg)
        {
            var splitMsg = CommandParser.Split(msg.Content);

            if (splitMsg[1] == "start")
            {
                await msg.RespondAsync(StartText(msg));
            }

            else if (splitMsg[1] == "stop")
            {
                await msg.RespondAsync(StopText(msg));
            }

            else
            {
                var game = Manager.GetGame(msg);

                if (Manager.GameExists(game))
                {
                    await msg.RespondAsync(ScreenText(msg, game));
                }
            }
            
        }
        
        private string StartText(DiscordMessage msg)
        {
            var started = Manager.AddGame(msg);
            
            if (!started)
                return "Game already taking place";

            else
            {
                TicTacToeGame game = Manager.GetGame(msg);
                char[,] screen = game.Screen;
                var translatedScreen = TranslateScreen(screen);

                return ScreenToString(translatedScreen);
            }
            
        }

        private string StopText(DiscordMessage msg)
        {
            var stopped = Manager.RemoveGame(msg);
            return !stopped ? "No game currently taking place" : "Game stopped";
        }
        
        private string ScreenText(DiscordMessage msg, TicTacToeGame game)
        {
            char[,] recievedScreen = StringToScreen(msg.Content);
                    
            char[,] screen = TranslateScreen(recievedScreen);
            Dictionary<char, char> differences = CompareScreens(screen, game.Screen);

            if (differences.Count != 1 || differences.ContainsKey('o') || differences.Values.Any(x => x != 'n'))
                return "Invalid Screen";
            
            else
            {
                game.UpdateScreen(screen);
                game.Move();
            }
            
            char[,] finalScreen = TranslateScreen(game.Screen);
            var finalString = ScreenToString(finalScreen);
            
            if (game.IsWin('x'))
            {
                Manager.RemoveGame(msg);
                return "You won!!";
            }

            if (game.IsWin('o'))
            {
                Manager.RemoveGame(msg);
                return finalString + "\nYou lost :(";
            }

            if (game.IsDraw())
            {
                Manager.RemoveGame(msg);
                return finalString + "\nGame drawn";
            }
            
            return finalString;
        }
        
        private Dictionary<char, char> CompareScreens(char[,] newScreen, char[,] oldScreen)
        {
            var differences = new Dictionary<char, char>();

            for (int i = 0; i < newScreen.GetLength(0); i++)
            {
                for (int j = 0; j < oldScreen.GetLength(1); j++)
                {
                    char newChar = newScreen[i, j];
                    char oldChar = oldScreen[i, j];
                    if (newChar != oldChar)
                        differences.Add(newChar, oldChar);
                }
            }

            return differences;
        }
        
        private char[,] TranslateScreen(char[,] screen)
        {
            char[,] retScreen = new char[screen.GetLength(0), screen.GetLength(1)];

            for (int i = 0; i < screen.GetLength(0); i++)
            {
                for (int j = 0; j < screen.GetLength(1); j++)
                {
                    retScreen[i, j] = ScreenTranslator[screen[i, j]];
                }
            }

            return retScreen;
        }
        
        private string ScreenToString(char[,] screen)
        {
            var str = "";
            
            for (int i = 0; i < screen.GetLength(0); i++)
            {
                for (int j = 0; j < screen.GetLength(1); j++)
                {
                    str += screen[i, j];
                }

                str += "\n";
            }

            return str;
        }

        private char[,] StringToScreen(string str)
        {
            str = str.Replace("\n", "");
            
            var screen = new char[3, 3];
            
            for (int i = 0; i < screen.GetLength(0); i++)
            {
                for (int j = 0; j < screen.GetLength(1); j++)
                {
                    var chr = str[i * 3 + j];
                    
                    screen[i, j] = chr;
                    //if (chr != '\n')
                        //screen[i, j] = str[i + j];
                }
            }

            return screen;
        }
    }
}