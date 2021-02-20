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
        Player GetWinner();
        bool IsGameDraw();
        bool IsGameOver();
        void SwitchPlayer();
        void PlayGame(int strike);
        void InitBoard(int blackCoins, int redCoins);
        void PrintScore();
      }
}
