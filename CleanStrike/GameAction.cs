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
            var playingHistory = player.StrikeHistory;
            bool isConsecutive3NoStrikes = false;
            int count = playingHistory.Count;
            int index = (count - KeyStore.GameSettings.Consecutive_No_Strike_Limit);

            if (count >= KeyStore.GameSettings.Consecutive_No_Strike_Limit)
            {
                isConsecutive3NoStrikes = true;
                while (index < count)
                {
                    if (!(playingHistory[index] == StrikeType.No_Strike))
                    {
                        isConsecutive3NoStrikes = false;
                        break;
                    }
                    index++;
                }
            }
            return isConsecutive3NoStrikes;
        }

        /**
         * Helper method check whether Foul or not.
         **/
        public bool CheckFoul(Player player)
        {
            var playingHistory = player.StrikeHistory;
            bool isFoul = false;
            int count = playingHistory.Count;
            int index = (count - KeyStore.GameSettings.Consecutive_Loosing_Limit);

            if (count >= KeyStore.GameSettings.Consecutive_Loosing_Limit)
            {
                bool isThreeNoStrike = CheckConsecutiveNoStrike(player);
                isFoul = true;
                while (index < count)
                {
                    if (!(IsLoosingPoint(playingHistory[index]) || isThreeNoStrike))
                    {
                        isFoul = false;
                        break;
                    }
                    index++;
                }
            }
            return isFoul;
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
        #endregion
    }
}
