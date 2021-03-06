﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStrike.Models
{
    /**
     * Model Class represent Common setting will used in Game.
     **/

    public class KeyStore
    {
        public static class Default
        {
            public static readonly int BlackCoins = 9;
            public static readonly int RedCoins = 1;
        }

        public static class GameSettings
        {
            public static readonly int Consecutive_No_Strike_Limit = 3;
            public static readonly int Consecutive_Loosing_Limit = 3;
            public static readonly int Min_Winining_Points = 5;
            public static readonly int Min_Win_Point_Diff = 3;
        }
        public static class Program 
        {
            public static readonly string FilePath = "CleanStrike.App.Input.input.txt";
        }
    }
}
