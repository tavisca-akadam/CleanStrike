using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStrike.Models
{
    public class CarromBoard
    {
        /**
         * Model Class representing Carrom board which used while playing game
         **/

        public CarromBoard()
        {

        }
        public CarromBoard(int blackCoins, int redCoins)
        {
            BlackCoins = blackCoins;
            RedCoins = redCoins;
        }
        public int BlackCoins { get; set; } = KeyStore.Default.BlackCoins;
        public int RedCoins { get; set; } = KeyStore.Default.RedCoins;
    }
}
