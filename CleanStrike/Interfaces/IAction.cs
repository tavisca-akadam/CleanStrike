using CleanStrike.Models;

namespace CleanStrike.Interfaces
{
    /**
     * It provides the all action will performs while playing game.
     **/

    public interface IAction
    {
        void RegisterAction(Team team, StrikeType strikeType);
        void OnCoinStriked(CarromBoard board);
        void OnRedCoinPocketed(CarromBoard board);
        bool CheckFoul(Team team);
        bool CheckConsecutiveNoStrike(Team team);
        void AddPoints(Team team, int point);
    }
}
