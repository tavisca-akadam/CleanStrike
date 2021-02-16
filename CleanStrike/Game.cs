using CleanStrike.Exceptions;
using CleanStrike.Interfaces;
using CleanStrike.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanStrike
{
    public class Game : IGame
    {
        private CarromBoard _carromBoard;

        private List<Player> _players;

        private int _currentPlayerIndex;
        private IAction _action;

        public Game(int blackCoins, int redCoins)
        {
            _carromBoard = new CarromBoard(blackCoins, redCoins);
            _players = new List<Player>();

            _players.Add(new Player("Ak"));
            _players.Add(new Player("Anil"));

            _currentPlayerIndex = 0;
            _action = new GameAction();
        }

        public Player GetWinner()
        {
            for(int itr = 0; itr < _players.Count; itr++) {
                if(IsPlayerWinner(itr))
                    return _players[itr];
            }

            return null;
        }

        //Method that decides if player winner from player list
        private bool IsPlayerWinner(int index) {
            if(_players[index].Score < KeyStore.GameSettings.Min_Winining_Points)
                return false;
            for(int itr = 0; itr < _players.Count; itr++) {
                if(index != itr) {
                    if(_players[index].Score - _players[itr].Score < KeyStore.GameSettings.Min_Win_Point_Diff)
                        return false;
                }
            }
            return true;
        }
        public bool IsGameDraw()
        {
            if(IsGameOver())
            {
                if (GetWinner() == null)
                    return true;
            }
            return false;
        }

        public bool IsGameOver() => (_carromBoard.BlackCoins == 0 && _carromBoard.RedCoins == 0);

        public void PrintScore()
        {
            Player winner = GetWinner();
            if(winner != null)
            {
                Console.WriteLine($"{winner.Name} won the game.");
                Console.WriteLine("Final Score:");
                for(int itr = 0; itr < _players.Count; itr++) {
                    Console.Write($"{_players[itr].Score}");
                }
            }
        }

        public void SwitchPlayer()
        {
            _currentPlayerIndex++;
            _currentPlayerIndex %= _players.Count;
        }

        public void PlayGame(int strikeType)
        {
                switch (strikeType)
                {  
                    case 1:
                        PassStriker(StrikeType.Strike, ScoreMap.assignedScore[StrikeType.Strike]);
                        break;
                    case 2:
                        PassStriker(StrikeType.Multi_Strike, ScoreMap.assignedScore[StrikeType.Multi_Strike]);
                        break;
                    case 3:
                        PassStriker(StrikeType.RedCoin_Strike, ScoreMap.assignedScore[StrikeType.RedCoin_Strike]);
                        _action.OnRedCoinPocketed(_carromBoard);
                        break;
                    case 4:
                        PassStriker(StrikeType.Striker_Strike, ScoreMap.assignedScore[StrikeType.Striker_Strike]);
                        break;
                    case 5:
                        PassStriker(StrikeType.Defunt_Coin, ScoreMap.assignedScore[StrikeType.Defunt_Coin]);
                        _action.OnCoinStriked(_carromBoard);
                        break;
                    case 6:
                        _action.RegisterAction(_players[_currentPlayerIndex], StrikeType.No_Strike);
                        SwitchPlayer();
                        break;

                }
                if (_action.CheckFoul(_players[_currentPlayerIndex]))
                    _action.AddPoints(_players[_currentPlayerIndex], (int)StrikeType.Foul);
                if (_action.CheckConsecutiveNoStrike(_players[_currentPlayerIndex]))
                    _action.AddPoints(_players[_currentPlayerIndex], (int)StrikeType.Consecutive_3_NoStrike);
            PrintScore();
        }

        private void PassStriker(StrikeType strikeType, int value)
        {
            _action.RegisterAction(_players[_currentPlayerIndex], strikeType);
            _action.AddPoints(_players[_currentPlayerIndex], value);
            SwitchPlayer();
        }
    }
}
