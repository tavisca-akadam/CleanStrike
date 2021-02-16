using CleanStrike.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStrike.Interfaces
{
    /**
     * It provides the all action will performs while playing game.
     **/

    public interface IAction
    {
        void RegisterAction(Player player, StrikeType strikeType);
        void OnCoinStriked(CarromBoard board);
        void OnRedCoinPocketed(CarromBoard board);
        bool CheckFoul(Player player);
        bool CheckConsecutiveNoStrike(Player player);
        void AddPoints(Player player, int point);
    }
}
