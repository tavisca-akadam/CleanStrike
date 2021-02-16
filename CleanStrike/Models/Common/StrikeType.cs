using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CleanStrike.Models
{
    public enum StrikeType
    {
        [Description("When player pockets a coin.")]
        Strike,

        [Description("When player pockets a more than one coin.")]
        Multi_Strike,

        [Description("When player pockets a red coin.")]
        RedCoin_Strike,

        [Description("When player pockets a striker coin.")]
        Striker_Strike,

        [Description("When coin is thrown out of the carrom board.")]
        Defunt_Coin,

        [Description("When player does not pocket a coin.")]
        No_Strike,

        [Description("When player loses at least a point for 3 succesive turns.")]
        Foul,

        [Description("When player does not pocket a coin for 3 successive turns.")]
        Consecutive_3_NoStrike
    }
}
