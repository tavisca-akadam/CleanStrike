using CleanStrike.Models;
using System;
using System.IO;
using System.Reflection;

namespace CleanStrike.App
{
    public class GameController
    {
        static void Main(string[] args)
        {
            try 
            {
                var inputStraem = ReadInputFromFile();      //Reading from file.
                int itr = 0;
                var inputArr = inputStraem.Split(',');      //Spliting file input.

                Game game = new Game(9, 1);
                int option = 7;
                while(true)
                {
                    if (itr >= inputArr.Length)
                        break;

                    option = Convert.ToInt32(inputArr[itr++]);
                    if (option >= 7)
                        break;

                    game.PlayGame(option);

                    if (game.GetWinner() != null)
                        break;
                    if (game.IsGameOver())
                    {
                        Console.WriteLine("Game Over...");
                        break;
                    }
                }


                game.PrintScore();
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Invalid input. Play Terminated.");
            }
        }
        
        /**
         * Method will use to read user input from file.
         **/
        static string ReadInputFromFile()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var streamReader = new StreamReader(assembly.GetManifestResourceStream(KeyStore.Program.FilePath));
            if (streamReader != null)
            {
                var text1 = streamReader.ReadLine();
                return text1;
            }

            return string.Empty;
        }      
    }
}
