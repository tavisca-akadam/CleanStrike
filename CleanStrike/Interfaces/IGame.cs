using CleanStrike.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStrike.Interfaces
{
    /**
     * All the methods requires for playing Carrom game resides here.
     **/

    public interface IGame
    {
        Team GetWinner();
        bool IsGameDraw();
        bool IsGameOver();
        void SwitchPlayer();
        void PlayGame(int strike);
      }
}
