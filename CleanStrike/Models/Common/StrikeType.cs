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
        Strike,
        Multi_Strike,
        RedCoin_Strike,
        Striker_Strike,
        Defunt_Coin,
        No_Strike,
        Foul,
        Consecutive_3_NoStrike
    }
}
