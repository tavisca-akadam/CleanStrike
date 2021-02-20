using CleanStrike.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using CleanStrike.Interfaces;
using CleanStrike.App.Extensions;

namespace CleanStrike.App
{
    public class GameController
    {
        private readonly IAction _action;
        private readonly IGame _game;
        public GameController(IAction action, IGame game)
        {
            _action = action;
            _game = game;
        }
        public void Play()
        {
            try
            {
                var inputStraem = ReadInputFromFile();      //Reading from file.
                int itr = 0;
                var inputArr = inputStraem.Split(',');      //Spliting file input.

                _game.InitBoard(9, 1);
                int option = 7;
                while (true)
                {
                    if (itr >= inputArr.Length)
                        break;

                    option = Convert.ToInt32(inputArr[itr++]);
                    if (option >= 7)
                        break;

                    _game.PlayGame(option);

                    if (_game.GetWinner() != null)
                        break;
                    if (_game.IsGameOver())
                    {
                        Console.WriteLine("Game Over...");
                        break;
                    }
                }


                _game.PrintScore();
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Something went wrong. Play Terminated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid input. Play Terminated.");
            }
        }
        static void Main(string[] args)
        {
            var host = AppBuilder.CreateHostBuilder(args).Build();
            host.Services.GetRequiredService<GameController>().Play();
        }
        
        /**
         * Method will use to read user input from file.
         **/
        static string ReadInputFromFile()
        {
            var assembly = Assembly.GetExecutingAssembly();
            try
            {
                var streamReader = new StreamReader(assembly.GetManifestResourceStream(KeyStore.Program.FilePath));
                if (streamReader != null)
                {
                    var text1 = streamReader.ReadLine();
                    return text1;
                }

            }
            catch(FileNotFoundException ex)
            {
                throw new FileNotFoundException();
            }

            return string.Empty;
        }      

        
    }
}
