using CleanStrike.Exceptions;
using CleanStrike.Models;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace CleanStrike.Tests
{
    public class GameActionFixture
    {
        
        [Theory]
        [InlineData(StrikeType.Consecutive_3_NoStrike, -1)]
        [InlineData(StrikeType.Defunt_Coin, -2)]
        [InlineData(StrikeType.Foul, -1)]
        [InlineData(StrikeType.Multi_Strike, 2)]
        [InlineData(StrikeType.No_Strike, 0)]
        [InlineData(StrikeType.RedCoin_Strike, 3)]
        [InlineData(StrikeType.Strike, 1)]
        [InlineData(StrikeType.Striker_Strike, -1)]
        public void Single_score_register_test(StrikeType strikeType, int expectedPoints)
        {
            GameAction gameAction = new GameAction();
            Player player = new Player("p1");
            Team testTeam = new Team("Test");
            testTeam.Players.Add(player);

            gameAction.AddPoints(testTeam, ScoreMap.AssignedScore[strikeType]);
            player.Score.Should().Be(expectedPoints);
        }

        [Fact]
        public void Multiple_score_register_test()
        {
            GameAction gameAction = new GameAction();
            Player player = new Player("p1");
            Team testTeam = new Team("Test");
            testTeam.Players.Add(player);

            gameAction.AddPoints(testTeam, ScoreMap.AssignedScore[StrikeType.RedCoin_Strike]);
            gameAction.AddPoints(testTeam, ScoreMap.AssignedScore[StrikeType.Multi_Strike]);
            gameAction.AddPoints(testTeam, ScoreMap.AssignedScore[StrikeType.Strike]);
            gameAction.AddPoints(testTeam, ScoreMap.AssignedScore[StrikeType.Defunt_Coin]);

            player.Score.Should().Be(4);
        }

        [Theory]
        [InlineData(StrikeType.Consecutive_3_NoStrike)]
        [InlineData(StrikeType.Defunt_Coin)]
        [InlineData(StrikeType.Foul)]
        [InlineData(StrikeType.Multi_Strike)]
        [InlineData(StrikeType.No_Strike)]
        [InlineData(StrikeType.RedCoin_Strike)]
        [InlineData(StrikeType.Strike)]
        [InlineData(StrikeType.Striker_Strike)]
        public void Single_action_entry_test(StrikeType strikeType)
        {
            GameAction gameAction = new GameAction();
            Player player = new Player("p1");
            Team testTeam = new Team("Test");
            testTeam.Players.Add(player);

            gameAction.RegisterAction(testTeam, strikeType);
            player.StrikeHistory.First().Should().Be(strikeType);
        }

        [Fact]
        public void Multiple_actions_entry_test()
        {
            GameAction gameAction = new GameAction();
            Player player = new Player("p1");
            Team testTeam = new Team("Test");
            testTeam.Players.Add(player);

            gameAction.RegisterAction(testTeam, StrikeType.RedCoin_Strike);
            gameAction.RegisterAction(testTeam, StrikeType.Multi_Strike);
            gameAction.RegisterAction(testTeam, StrikeType.Strike);
            gameAction.RegisterAction(testTeam, StrikeType.Defunt_Coin);

            player.StrikeHistory.Count.Should().Be(4);
            player.StrikeHistory.First().Should().Be(StrikeType.RedCoin_Strike);
            player.StrikeHistory.Last().Should().Be(StrikeType.Defunt_Coin);
        }

        [Fact]
        public void Check_foul_test()
        {
            GameAction gameAction = new GameAction();
            Player player = new Player("p1");
            Team testTeam = new Team("Test");
            testTeam.Players.Add(player);

            gameAction.RegisterAction(testTeam, StrikeType.Striker_Strike);
            gameAction.RegisterAction(testTeam, StrikeType.Defunt_Coin);
            gameAction.RegisterAction(testTeam, StrikeType.Striker_Strike);

            gameAction.CheckFoul(testTeam).Should().BeTrue();
        }

        [Fact]
        public void Check_three_no_consecutive_strikes_test()
        {
            GameAction gameAction = new GameAction();
            Player player = new Player("p1");
            Team testTeam = new Team("Test");
            testTeam.Players.Add(player);

            gameAction.RegisterAction(testTeam, StrikeType.No_Strike);
            gameAction.RegisterAction(testTeam, StrikeType.No_Strike);
            gameAction.RegisterAction(testTeam, StrikeType.No_Strike);

            gameAction.CheckConsecutiveNoStrike(testTeam).Should().BeTrue();
        }

        [Fact]
        public void Coin_removed_test()
        {
            CarromBoard carromBoard = new CarromBoard();
            GameAction gameAction = new GameAction();

            gameAction.OnCoinStriked(carromBoard);

            carromBoard.BlackCoins.Should().Be(KeyStore.Default.BlackCoins - 1);
        }

        [Fact]
        public void RedCoin_pocketed_test()
        {
            CarromBoard carromBoard = new CarromBoard();
            GameAction gameAction = new GameAction();

            gameAction.OnRedCoinPocketed(carromBoard);

            carromBoard.RedCoins.Should().Be(KeyStore.Default.RedCoins - 1);
        }

        [Fact]
        public void NoCoin_on_a_board_test()
        {
            CarromBoard carromBoard = new CarromBoard();
            GameAction gameAction = new GameAction();
            carromBoard.BlackCoins = 0;
            carromBoard.RedCoins = 0;

            Assert.Throws<CoinNotFoundException>(() => gameAction.OnCoinStriked(carromBoard));
        }

        [Fact]
        public void NoRedCoin_on_a_board_test()
        {
            CarromBoard carromBoard = new CarromBoard();
            GameAction gameAction = new GameAction();
            carromBoard.RedCoins = 0;

            Assert.Throws<CoinNotFoundException>(() => gameAction.OnRedCoinPocketed(carromBoard));
        }
    }
}
