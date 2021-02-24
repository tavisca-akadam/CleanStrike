using CleanStrike.Interfaces;
using CleanStrike.Models;
using System;
using System.Collections.Generic;

namespace CleanStrike
{
    public class Game : IGame
    {
        private CarromBoard _carromBoard;
        private List<Team> _teams;
        private int _currentTeamIndex;
        private IAction _action;

        public Game(int blackCoins, int redCoins)
        {
            _carromBoard = new CarromBoard(blackCoins, redCoins);
            _teams = new List<Team>();

            // Register Teams 
            _teams.Add(new Team("Rangers"));
            _teams.Add(new Team("Blasters"));

            // Register Team Players
            _teams[0].Players.Add(new Player("Playe1"));
            _teams[0].Players.Add(new Player("Playe3"));

            _teams[1].Players.Add(new Player("Playe2"));
            _teams[1].Players.Add(new Player("Playe4"));

            // Initialize current index as 0, so first team1 approach first.
            _currentTeamIndex = 0;

            _action = new GameAction();
        }

        /*
         * Helper method to return game final winner of game.
         **/
        public Team GetWinner()
        {
            for(int itr = 0; itr < _teams.Count; itr++) {
                if(IsTeamWinner(itr))
                    return _teams[itr];
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
            Team winner = GetWinner();
            if (winner != null)
            {
                Console.Write($"{winner.Name} won the game. ");
            }
            Console.WriteLine("Final Score:");
            for (int itr = 0; itr < _teams.Count; itr++)
            {
                Console.WriteLine($"{_teams[itr].Name} : {_teams[itr].TotalScore}.");
            } 
        }

        /**
         * Helper method for switching player's turn
         **/
        public void SwitchPlayer()
        {
            _teams[_currentTeamIndex].CurrentPlayerIndex++;
            _teams[_currentTeamIndex].CurrentPlayerIndex %= _teams[_currentTeamIndex].Players.Count;
            _currentTeamIndex++;
            _currentTeamIndex %= _teams.Count;
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
                        MakeMove(StrikeType.Strike, ScoreMap.AssignedScore[StrikeType.Strike]);
                        break;
                    case 2:
                        MakeMove(StrikeType.Multi_Strike, ScoreMap.AssignedScore[StrikeType.Multi_Strike]);
                        break;
                    case 3:
                        MakeMove(StrikeType.RedCoin_Strike, ScoreMap.AssignedScore[StrikeType.RedCoin_Strike]);
                        _action.OnRedCoinPocketed(_carromBoard);
                        break;
                    case 4:
                        MakeMove(StrikeType.Striker_Strike, ScoreMap.AssignedScore[StrikeType.Striker_Strike]);
                        break;
                    case 5:
                        MakeMove(StrikeType.Defunt_Coin, ScoreMap.AssignedScore[StrikeType.Defunt_Coin]);
                        _action.OnCoinStriked(_carromBoard);
                        break;
                    case 6:
                        MakeMove(StrikeType.No_Strike, ScoreMap.AssignedScore[StrikeType.No_Strike]);
                        break;

                }
                if (_action.CheckFoul(_teams[_currentTeamIndex]))
                    _action.AddPoints(_teams[_currentTeamIndex], ScoreMap.AssignedScore[StrikeType.Foul]);
                if (_action.CheckConsecutiveNoStrike(_teams[_currentTeamIndex]))
                    _action.AddPoints(_teams[_currentTeamIndex], ScoreMap.AssignedScore[StrikeType.Consecutive_3_NoStrike]);

               SwitchPlayer();
        }

        #region private methods
        // This method will register the move and pass the striker to next player.
        private void MakeMove(StrikeType strikeType, int value)
        {
            _action.RegisterAction(_teams[_currentTeamIndex], strikeType);
            _action.AddPoints(_teams[_currentTeamIndex], value);
        }

        //Method that decides if player is winner from player list
        private bool IsTeamWinner(int index) {
            if(_teams[index].TotalScore < KeyStore.GameSettings.Min_Winining_Points)
                return false;
            for(int itr = 0; itr < _teams.Count; itr++) {
                if(index != itr) {
                    if(_teams[index].TotalScore - _teams[itr].TotalScore < KeyStore.GameSettings.Min_Win_Point_Diff)
                        return false;
                }
            }
            return true;
        }
        #endregion
    }
}
