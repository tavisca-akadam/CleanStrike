using CleanStrike.Exceptions;
using CleanStrike.Interfaces;
using CleanStrike.Models;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CleanStrike.Tests
{
    public class CleanStrikeGameFixture
    {

        [Fact]
        public void Check_game_is_draw_test()
        {
            var game = GetGameObj();
            game.InitBoard(0, 0);

            game.IsGameDraw().Should().Be(true);
        }

        [Fact]
        public void Check_game_is_over_test()
        {
            var game = GetGameObj();
            game.InitBoard(0, 0);

            var result = game.IsGameOver();
            result.Should().Be(true);
        }

        [Fact]
        public void GetWinner_test()
        {
            Player winner = null;
            int[] userInput = { 3, 1, 2, 2, 2, 1 };
            var game = GetGameObj();
            game.InitBoard(9, 1);

            foreach (var i in userInput)
            {
                game.PlayGame(i);
                winner = game.GetWinner();
                if (winner != null)
                    break;

            }
            winner.Should().NotBeNull();
        }

        [Fact]
        public void Check_game_is_over_using_input_test()
        {
            int[] userInput = { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 };
            var game = GetGameObj();
            game.InitBoard(9, 1);

            foreach (var i in userInput)
            {
                game.PlayGame(i);
                if (game.IsGameOver())
                    break;

            }
            game.IsGameOver().Should().BeTrue();
        }

        [Fact]
        public void Validate_Coin_not_found_exception_test()
        {
            int[] userInput = { 3, 3 };

            var game = GetGameObj();
            game.InitBoard(9, 1);
            try
            {
                foreach (var i in userInput)
                {
                    game.PlayGame(i);
                }
            }
            catch (Exception ex)
            {
                ex.GetType().Should().Be(typeof(CoinNotFoundException));
            }
        }

        private Game GetGameObj()
        {
            var mockGameAction = new Mock<IAction>();

            var gameObj =  new Game(mockGameAction.Object);
            
            return gameObj;
        }
    }
}
