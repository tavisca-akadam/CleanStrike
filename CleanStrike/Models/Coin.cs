using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStrike.Models
{
    public class Coin
    {
        public Coin(int count)
        {
            this.Count = count;
        }
        public int Count { get; set; }
    }
}
