using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStrike.Models
{
    public class CarromBoard
    {
        public RedCoin RedCoins { get; } = new RedCoin();
        public BlackCoin BlackCoins { get; } = new BlackCoin(KeyStore.Default.BlackCoins);

    }
}
