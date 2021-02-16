using System.Collections.Generic;

namespace CleanStrike.Models
{
    /**
     * Model Class to represent score for respective move.
     **/
    
    public static class ScoreMap {
        public static Dictionary<StrikeType, int> AssignedScore { get; }
        static ScoreMap()
        {
                AssignedScore = new Dictionary<StrikeType, int>();
                AssignedScore.Add(StrikeType.Strike, 1);
                AssignedScore.Add(StrikeType.Multi_Strike, 2);
                AssignedScore.Add(StrikeType.RedCoin_Strike, 3);
                AssignedScore.Add(StrikeType.Striker_Strike, -1);
                AssignedScore.Add(StrikeType.Defunt_Coin, -2);
                AssignedScore.Add(StrikeType.No_Strike, 0);
                AssignedScore.Add(StrikeType.Foul, -1);
                AssignedScore.Add(StrikeType.Consecutive_3_NoStrike, -1);
        }
    }
}