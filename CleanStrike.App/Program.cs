using CleanStrike.Models;
using System;

namespace CleanStrike.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            game.Print();

            game.Play();
        }
        
        
    }
}
