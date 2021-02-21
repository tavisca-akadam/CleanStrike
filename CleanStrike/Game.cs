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

        private List<Team> teams;

        private int _currentTeamIndex;

        private IAction _action;

        public Game(int blackCoins, int redCoins)
        {
            _carromBoard = new CarromBoard(blackCoins, redCoins);
            teams = new List<Team>();


            teams.Add(new Team("Rockers"));
            teams.Add(new Team("Thunders"));
            teams.Add(new Team("Destroyers"));

            teams[0].Players.Add(new Player("Player1"));
            teams[1].Players.Add(new Player("Player2"));
            teams[2].Players.Add(new Player("Player3"));

            //teams[0].Players.Add(new Player("Player1"));
            //teams[0].Players.Add(new Player("Player3"));

            //teams[1].Players.Add(new Player("Player2"));
            //teams[1].Players.Add(new Player("Player4"));

            _currentTeamIndex = 0;
            _action = new GameAction();
        }

        /*
         * Helper method to return game final winner of game.
         **/
        public Team GetWinner()
        {
            for(int itr = 0; itr < teams.Count; itr++) {
                if(IsTeamWinner(itr))
                    return teams[itr];
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
            for (int itr = 0; itr < teams.Count; itr++)
            {
                Console.WriteLine($"{teams[itr].Name} : {teams[itr].TotalScore}.");
            } 
        }

        /**
         * Helper method for switching player's turn
         **/
        public void SwitchPlayer()
        {
            teams[_currentTeamIndex].CurrentPlayerIndex++;
            teams[_currentTeamIndex].CurrentPlayerIndex %= teams[_currentTeamIndex].Players.Count;
            _currentTeamIndex++;
            _currentTeamIndex %= teams.Count;
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
                        _action.RegisterAction(teams[_currentTeamIndex], StrikeType.No_Strike);
                        SwitchPlayer();
                        break;

                }
                if (_action.CheckFoul(teams[_currentTeamIndex]))
                    _action.AddPoints(teams[_currentTeamIndex], ScoreMap.AssignedScore[StrikeType.Foul]);
                if (_action.CheckConsecutiveNoStrike(teams[_currentTeamIndex]))
                    _action.AddPoints(teams[_currentTeamIndex], ScoreMap.AssignedScore[StrikeType.Consecutive_3_NoStrike]);
        }

        #region private methods
        // This method will register the move and pass the striker to next player.
        private void PassStriker(StrikeType strikeType, int value)
        {
            _action.RegisterAction(teams[_currentTeamIndex], strikeType);
            _action.AddPoints(teams[_currentTeamIndex], value);
            SwitchPlayer();
        }

        //Method that decides if player is winner from player list
        private bool IsTeamWinner(int index)
        {
            if (teams[index].TotalScore < KeyStore.GameSettings.Min_Winining_Points)
                return false;
            for (int itr = 0; itr < teams.Count; itr++)
            {
                if (index != itr)
                {
                    if (teams[index].TotalScore - teams[itr].TotalScore < KeyStore.GameSettings.Min_Win_Point_Diff)
                        return false;
                }
            }
            return true;
        }
        #endregion
    }
}
