using CleanStrike.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStrike.Interfaces
{
    public interface IGame
    {
        void Start();
        void RegisterScore(int score);
        bool IsConsecutiveNoStrike();
        bool IsFoul();
        Player GetWinner();
        bool IsGameDraw();
        bool IsGameOver();
        void RegisterMove(StrikeType strikeType);
        void SwitchPlayer();
        void CoinStriked();
        void StrikerStriked();
        void RedCoinStriked();
      }
}
