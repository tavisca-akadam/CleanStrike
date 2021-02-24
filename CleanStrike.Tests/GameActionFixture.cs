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
            Team team = new Team("test");
            Player player = new Player("p1");
            team.Players.Add(player);

            gameAction.AddPoints(team, ScoreMap.AssignedScore[strikeType]);
            team.TotalScore.Should().Be(expectedPoints);
        }

        [Fact]
        public void Multiple_score_register_test()
        {
            GameAction gameAction = new GameAction();
            Team team = new Team("test");
            Player player = new Player("p1");
            team.Players.Add(player);

            gameAction.AddPoints(team, ScoreMap.AssignedScore[StrikeType.RedCoin_Strike]);
            gameAction.AddPoints(team, ScoreMap.AssignedScore[StrikeType.Multi_Strike]);
            gameAction.AddPoints(team, ScoreMap.AssignedScore[StrikeType.Strike]);
            gameAction.AddPoints(team, ScoreMap.AssignedScore[StrikeType.Defunt_Coin]);

            team.TotalScore.Should().Be(4);
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
            Team team = new Team("test");
            Player player = new Player("p1");
            team.Players.Add(player);

            gameAction.RegisterAction(team, strikeType);
            team.PlayHistory.First().Should().Be(strikeType);
        }

        [Fact]
        public void Multiple_actions_entry_test()
        {
            GameAction gameAction = new GameAction();
            Team team = new Team("test");
            Player player = new Player("p1");
            team.Players.Add(player);

            gameAction.RegisterAction(team, StrikeType.RedCoin_Strike);
            gameAction.RegisterAction(team, StrikeType.Multi_Strike);
            gameAction.RegisterAction(team, StrikeType.Strike);
            gameAction.RegisterAction(team, StrikeType.Defunt_Coin);

            team.PlayHistory.Count.Should().Be(4);
            team.PlayHistory.First().Should().Be(StrikeType.RedCoin_Strike);
            team.PlayHistory.Last().Should().Be(StrikeType.Defunt_Coin);
            player.StrikeHistory.First().Should().Be(StrikeType.RedCoin_Strike);
            player.StrikeHistory.Last().Should().Be(StrikeType.Defunt_Coin);
        }

        [Fact]
        public void Check_foul_test()
        {
            GameAction gameAction = new GameAction();
            Team team = new Team("test");
            Player player = new Player("p1");
            team.Players.Add(player);

            gameAction.RegisterAction(team, StrikeType.Striker_Strike);
            gameAction.RegisterAction(team, StrikeType.Defunt_Coin);
            gameAction.RegisterAction(team, StrikeType.Striker_Strike);

            gameAction.CheckFoul(team).Should().BeTrue();
        }

        [Fact]
        public void Check_three_no_consecutive_strikes_test()
        {
            GameAction gameAction = new GameAction();
            Team team = new Team("test");
            Player player = new Player("p1");
            team.Players.Add(player);

            gameAction.RegisterAction(team, StrikeType.No_Strike);
            gameAction.RegisterAction(team, StrikeType.No_Strike);
            gameAction.RegisterAction(team, StrikeType.No_Strike);

            gameAction.CheckConsecutiveNoStrike(team).Should().BeTrue();
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
