using CleanStrike.Exceptions;
using CleanStrike.Interfaces;
using CleanStrike.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStrike
{
    public class GameAction : IAction
    {
        /*
         * To maintain the Game playing history
         **/
        public List<StrikeType> gameHistory;
        public GameAction() => gameHistory = new List<StrikeType>();

        /**
         * Helper Method to check if consecutive three no strike move played by same player.
         **/
        public bool CheckConsecutiveNoStrike(Player player)
        {
            if (player.NoStrikeCount >= KeyStore.GameSettings.Consecutive_No_Strike_Limit)
                return true;
            return false;
        }

        /**
         * Helper method check whether Foul or not.
         **/
        public bool CheckFoul(Player player)
        {
            if (player.FoulCount >= KeyStore.GameSettings.Consecutive_Loosing_Limit)
                return true;
            return false;
        }

        /**
         * Helper method to decrement coin count when condition met.
         **/
        public void OnCoinStriked(CarromBoard board)
        {
            if (board.BlackCoins != 0)
                board.BlackCoins--;
            else if (board.RedCoins != 0)
                board.RedCoins--;
            else
                throw new CoinNotFoundException();
        }

        /**
         * Helper method to decrement red coin count when condition met.
         **/
        public void OnRedCoinPocketed(CarromBoard board)
        {
            if (board.RedCoins != 0)
                board.RedCoins--;
            else
                throw new CoinNotFoundException();
        }

        /**
         * Method Register move for given player.
         **/
        public void RegisterAction(Player player, StrikeType strikeType)
        {
            player.StrikeHistory.Add(strikeType);
            SetNoStrikeMoveCount(strikeType, player);
            SetFoulMoveCount(strikeType, player);
            gameHistory.Add(strikeType);
        }

        /**
         * Method increase/decrease player's score.
         **/
        public void AddPoints(Player player, int points) => player.Score += points;

        #region Private Methods
        /**
         *  Method decides given move (strikeType) has loosing point.
         **/
        private static bool IsLoosingPoint(StrikeType strikeType)
        {
            return ((strikeType == StrikeType.Defunt_Coin) ||
                (strikeType == StrikeType.Striker_Strike));
        }

        /**
         * Handler method set NoStrikeCount property according to striker's move.
         **/
        private void SetNoStrikeMoveCount(StrikeType strikeType, Player player)
        {
            player.NoStrikeCount = strikeType == StrikeType.No_Strike ? player.NoStrikeCount + 1 : 0;
        }

        /**
         * Handler method set FoulCount property according to striker's move.
         **/
        private void SetFoulMoveCount(StrikeType strikeType, Player player)
        {
            player.FoulCount = (IsLoosingPoint(strikeType) || CheckConsecutiveNoStrike(player)) ? player.FoulCount + 1 : 0;
        }
        #endregion
    }
}
