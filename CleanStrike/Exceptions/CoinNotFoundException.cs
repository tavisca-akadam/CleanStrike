using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStrike.Exceptions
{
    [Serializable]
    public class CoinNotFoundException : Exception
    {
        /*
         * Exception if Coin not found or not present on board.
         **/

        public CoinNotFoundException()
        {

        }
    }
}
