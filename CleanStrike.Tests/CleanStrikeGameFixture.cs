using CleanStrike.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CleanStrike.Tests
{
    public class CleanStrikeGameFixture
    {
        private static Player _player1;
        [Fact]
        public void DummyTest()
        {
            return;
        }

        [Fact]
        public void Test()
        {
            Game game = new Game(9, 1);

            

            game.GetWinner();
        }
    }
}
