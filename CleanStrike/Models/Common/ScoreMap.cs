using System.Collections.Generic;

namespace CleanStrike.Models
{
    /**
     * Model Class to represent score for respective move.
     **/
    
    public static class ScoreMap {
        public static Dictionary<StrikeType, int> assignedScore { get; }
        static ScoreMap()
        {
                assignedScore = new Dictionary<StrikeType, int>();
                assignedScore.Add(StrikeType.Strike, 1);
                assignedScore.Add(StrikeType.Multi_Strike, 2);
                assignedScore.Add(StrikeType.RedCoin_Strike, 3);
                assignedScore.Add(StrikeType.Striker_Strike, -1);
                assignedScore.Add(StrikeType.Defunt_Coin, -2);
                assignedScore.Add(StrikeType.No_Strike, 0);
                assignedScore.Add(StrikeType.Foul, -1);
                assignedScore.Add(StrikeType.Consecutive_3_NoStrike, -1);
        }
    }
}