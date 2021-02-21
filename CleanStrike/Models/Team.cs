using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStrike.Models
{
    public class Team
    {
        public Team(string teamName)
        {
            Name = teamName;
        }
        public string Name { get; }
        public int Size { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();   
        public List<StrikeType> PlayingHistory { get; set; } = new List<StrikeType>();
        public int TotalScore { get; set; } = 0;
        public int CurrentPlayerIndex { get; set; } = 0;
    }
}
