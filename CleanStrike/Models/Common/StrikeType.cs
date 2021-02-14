using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CleanStrike.Models
{
    public enum StrikeType
    {
        //TODO: Add Description
        [Description("")]
        Strike = 1,
        Multi_Strike = 2,
        RedCoin_Strike = 3,
        Striker_Strike = -1,
        Defunt_Coin = -2,
        No_Strike = 0,
        Foul = -1,
        Consecutive_3_NoStrike = -1
    }
}
