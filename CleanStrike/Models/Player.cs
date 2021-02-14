using CleanStrike.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStrike.Models
{
    public class Player
    {
        public Player(string name, bool isTurn)
        {
            Name = name;
            IsPlayTurn = isTurn;
        }
        public string Name { get; set; } = Guid.NewGuid().ToString();
        public bool IsPlayTurn { get; set; } = false;
        public int Score { get; set; } = 0;

        public List<StrikeType> StrikeHistory { get; set; } = new List<StrikeType>();
        public void ChnageTurn()
        {
            this.IsPlayTurn = !this.IsPlayTurn;
        }
    }
}
