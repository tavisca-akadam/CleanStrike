using CleanStrike.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CleanStrike.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = ReadInputFromFile();
            int i = 0;
            var inp = s.Split(',');

            var inputINTArr = inp.Select(x => Convert.ToInt16(x)).ToArray();

            Game game = new Game(9, 1);
            int option = 7;
            do
            {
                Console.WriteLine("Choose from the list");
                Console.WriteLine("1. STRIKE");
                Console.WriteLine("2. MultiSTRIKE");
                Console.WriteLine("3. Red STRIKE");
                Console.WriteLine("4. Striker STRIKE");
                Console.WriteLine("5. Defunct Coin");
                Console.WriteLine("6. None");
                Console.WriteLine("7. Exit");

                option = Convert.ToInt32(inp[i++]);
                if (option >= 7)
                    break;
                game.PlayGame(option);

                if (game.GetWinner() != null)
                {
                    break;
                }
            } while (!(game.IsGameOver() || option == 7));

        }
        
        public static string ReadInputFromFile()
        {
            var a = Assembly.GetExecutingAssembly();



            var path = "CleanStrike.App.Input.input.txt";
            var streamReader = new StreamReader(a.GetManifestResourceStream(path));
            if (streamReader != null)
            {
                var text1 = streamReader.ReadToEnd();
                return text1;
            }

            return string.Empty;
        }
        
    }
}
