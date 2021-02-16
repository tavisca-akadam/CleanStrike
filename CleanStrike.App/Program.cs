using CleanStrike.Models;
using System;
using System.IO;
using System.Reflection;

namespace CleanStrike.App
{
    class Program
    {
        static void Main(string[] args)
        {
            try 
            {
                var inputStraem = ReadInputFromFile();
                int itr = 0;
                var inputArr = inputStraem.Split(',');

                Game game = new Game(9, 1);
                int option = 7;
                do
                {
                    option = Convert.ToInt32(inputArr[itr++]);
                    if (option >= 7)
                        break;

                    game.PlayGame(option);

                    if (game.GetWinner() != null)
                        break;
                    
                } while (!(game.IsGameOver() || option >= 7));
                
                game.PrintScore();
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Invalid input. Play Terminated.");
            }
        }
        
        public static string ReadInputFromFile()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var streamReader = new StreamReader(assembly.GetManifestResourceStream(KeyStore.Program.FilePath));
            if (streamReader != null)
            {
                var text1 = streamReader.ReadLine(); //TODO: Need to change read score by score
                return text1;
            }

            return string.Empty;
        }
        
    }
}
