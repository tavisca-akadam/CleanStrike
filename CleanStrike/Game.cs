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

            _players.Add(new Player("Playe1"));
            _players.Add(new Player("Player2"));

            _currentPlayerIndex = 0;
            _action = new GameAction();
        }

        /*
         * Helper method to return game final winner of game.
         **/
        public Player GetWinner()
        {
            for(int itr = 0; itr < _players.Count; itr++) {
                if(IsPlayerWinner(itr))
                    return _players[itr];
            }
            return null;
        }

        /*
         * Helper method to return game is draw or not.
         **/
        public bool IsGameDraw()
        {
            if(IsGameOver())
            {
                if (GetWinner() == null)
                    return true;
            }
            return false;
        }

        /*
         * Helper method to return game is over or not.
         **/
        public bool IsGameOver() => (_carromBoard.BlackCoins == 0 && _carromBoard.RedCoins == 0);

        /*
         * Helper Method to printing the scores of players.
         **/
        public void PrintScore()
        {
            Player winner = GetWinner();
            if (winner != null)
            {
                Console.Write($"{winner.Name} won the game. ");
            }
            Console.WriteLine("Final Score:");
            for (int itr = 0; itr < _players.Count; itr++)
            {
                Console.WriteLine($"{_players[itr].Name} : {_players[itr].Score}.");
            } 
        }

        /**
         * Helper method for switching player's turn
         **/
        public void SwitchPlayer()
        {
            _currentPlayerIndex++;
            _currentPlayerIndex %= _players.Count;
        }

        /**
         * Entry Method
         * Decides game movement using player's move(strikeType).
         **/
        public void PlayGame(int strikeType)
        {
                switch (strikeType)
                {  
                    case 1:
                        PassStriker(StrikeType.Strike, ScoreMap.AssignedScore[StrikeType.Strike]);
                        break;
                    case 2:
                        PassStriker(StrikeType.Multi_Strike, ScoreMap.AssignedScore[StrikeType.Multi_Strike]);
                        break;
                    case 3:
                        PassStriker(StrikeType.RedCoin_Strike, ScoreMap.AssignedScore[StrikeType.RedCoin_Strike]);
                        _action.OnRedCoinPocketed(_carromBoard);
                        break;
                    case 4:
                        PassStriker(StrikeType.Striker_Strike, ScoreMap.AssignedScore[StrikeType.Striker_Strike]);
                        break;
                    case 5:
                        PassStriker(StrikeType.Defunt_Coin, ScoreMap.AssignedScore[StrikeType.Defunt_Coin]);
                        _action.OnCoinStriked(_carromBoard);
                        break;
                    case 6:
                        _action.RegisterAction(_players[_currentPlayerIndex], StrikeType.No_Strike);
                        SwitchPlayer();
                        break;

                }
                if (_action.CheckFoul(_players[_currentPlayerIndex]))
                    _action.AddPoints(_players[_currentPlayerIndex], ScoreMap.AssignedScore[StrikeType.Foul]);
                if (_action.CheckConsecutiveNoStrike(_players[_currentPlayerIndex]))
                    _action.AddPoints(_players[_currentPlayerIndex], ScoreMap.AssignedScore[StrikeType.Consecutive_3_NoStrike]);
        }

        #region private methods
        // This method will register the move and pass the striker to next player.
        private void PassStriker(StrikeType strikeType, int value)
        {
            _action.RegisterAction(_players[_currentPlayerIndex], strikeType);
            _action.AddPoints(_players[_currentPlayerIndex], value);
            SwitchPlayer();
        }

        //Method that decides if player is winner from player list
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
        #endregion
    }
}
