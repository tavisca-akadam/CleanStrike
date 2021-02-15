using CleanStrike.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStrike.Interfaces
{
    public interface IGame
    {
        Player GetWinner();
        bool IsGameDraw();
        bool IsGameOver();
        void SwitchPlayer();
        void PlayGame(int strike);
      }
}
