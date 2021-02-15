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
        public List<StrikeType> gameHistory;
        public GameAction() => gameHistory = new List<StrikeType>();

        public bool CheckConsecutiveNoStrike(Player player)
        {
            var playingHistory = player.StrikeHistory;
            bool isConsecutive3NoStrikes = false;
            int count = playingHistory.Count;
            int index = (count - KeyStore.GameSettings.Consecutive_No_Strike_Limit);

            if (count >= KeyStore.GameSettings.Consecutive_No_Strike_Limit)
            {
                isConsecutive3NoStrikes = true;
                for (; index < count; index++)
                {
                    if (!(playingHistory[index] == StrikeType.No_Strike))
                    {
                        isConsecutive3NoStrikes = false;
                        break;
                    }
                }
            }
            return isConsecutive3NoStrikes;
        }

        public bool CheckFoul(Player player)
        {
            var playingHistory = player.StrikeHistory;
            bool isFoul = false;
            int count = playingHistory.Count;
            int index = (count - KeyStore.GameSettings.Consecutive_Loosing_Limit);

            if (count >= KeyStore.GameSettings.Consecutive_Loosing_Limit)
            {
                isFoul = true;
                for (; index < count; index++)
                {
                    if (!IsLoosingPoint(playingHistory[index]))
                    {
                        isFoul = false;
                        break;
                    }
                }
            }
            return isFoul;
        }

        public void OnCoinStriked(CarromBoard board)
        {
            if (board.BlackCoins != 0)
                board.BlackCoins--;
            else if (board.RedCoins != 0)
                board.RedCoins--;
            else
                throw new CoinNotFoundException();
        }

        public void OnRedCoinPocketed(CarromBoard board)
        {
            if (board.RedCoins != 0)
                board.RedCoins--;
            else
                throw new CoinNotFoundException();
        }

        public void RegisterAction(Player player, StrikeType strikeType)
        {
            player.StrikeHistory.Add(strikeType);
            gameHistory.Add(strikeType);
        }

        public void AddPoints(Player player, int points) => player.Score += points;

        #region Private Methods
        private static bool IsLoosingPoint(StrikeType strikeType)
        {
            return ((strikeType == StrikeType.Consecutive_3_NoStrike) || (strikeType == StrikeType.Defunt_Coin) ||
                (strikeType == StrikeType.Striker_Strike));
        }

        #endregion
    }
}
