
using System.Collections.Generic;

namespace CleanStrike.Models 
{
    public class Team
    {
        public Team(string teamName)
        {
            Name = teamName;
        }

        public string Name { get; set; }
        public int TotalScore { get; set; } = 0;
        public List<StrikeType> PlayHistory { get; set; } = new List<StrikeType>();
        public List<Player> Players { get; set; } = new List<Player>();
        public int NoStrikeCount { get; set; } = 0;
        public int FoulCount { get; set; } = 0;
        public int CurrentPlayerIndex { get; set; } = 0;
    }
}