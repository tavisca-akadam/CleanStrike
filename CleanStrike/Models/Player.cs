using CleanStrike.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStrike.Models
{
    public class Player
    {
        /**
         * Model Class representing single player.
         **/
        public Player(string name)
        {
            Name = name;
        }
        public string Name { get; set; } = Guid.NewGuid().ToString();
        public bool Won { get; set; } = false;
        public int Score { get; set; } = 0;

        public List<StrikeType> StrikeHistory { get; set; } = new List<StrikeType>();
        public int FoulCount { get; set; } = 0;
        public int NoStrikeCount { get; set; } = 0;
    }
}
