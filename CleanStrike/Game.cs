using CleanStrike.Exceptions;
using CleanStrike.Interfaces;
using CleanStrike.Models;
using System;
using System.Collections.Generic;

namespace CleanStrike
{
    public class Game : IGame
    {
        private CarromBoard _carromBoard;
        private Player _player1;
        private Player _player2;
        private Player _currentPlayer;

        private Queue<Player> _playerQueues;
        private IAction _action;

        public Game(int blackCoins, int redCoins)
        {
            _playerQueues = new Queue<Player>();
            _carromBoard = new CarromBoard(blackCoins, redCoins);
            _player1 = new Player("Anil");
            _player2 = new Player("Ak");
            _action = new GameAction();
            InitBoard();
        }

        public void InitBoard()
        {
            _playerQueues.Enqueue(_player2);
            _playerQueues.Enqueue(_player1);
            _currentPlayer = _player1;
        }

        public Player GetWinner()
        {
            if (((_player1.Score >= KeyStore.GameSettings.Min_Winining_Points) || (_player2.Score >= KeyStore.GameSettings.Min_Winining_Points))
                && Math.Abs(_player1.Score - _player2.Score) >= KeyStore.GameSettings.Min_Win_Point_Diff)
            {
                Player winner =  _player1.Score > _player2.Score ? _player1 : _player2;
                winner.Won = true;
                return winner;
            }
            return null;
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
                Console.WriteLine($"{winner.Name} won the game. Final Score: {_player1.Score}-{_player2.Score}.");
                return;
            }
            Console.WriteLine($"Current Score: {_player1.Score}-{_player2.Score}.");
            Console.WriteLine($"Coins Status: [B: {_carromBoard.BlackCoins}] [R:{_carromBoard.RedCoins}].");
        }

        public void SwitchPlayer()
        {
            _currentPlayer = _playerQueues.Dequeue();
            _playerQueues.Enqueue(_currentPlayer);
        }

        public void PlayGame(int strikeType)
        {
               
                switch (strikeType)
                {  
                    case 1:
                        PassStriker(StrikeType.Strike);
                        break;
                    case 2:
                        PassStriker(StrikeType.Multi_Strike);
                        break;
                    case 3:
                        PassStriker(StrikeType.RedCoin_Strike);
                        _action.OnRedCoinPocketed(_carromBoard);
                        break;
                    case 4:
                        PassStriker(StrikeType.Striker_Strike);
                        break;
                    case 5:
                        PassStriker(StrikeType.Defunt_Coin);
                        _action.OnCoinStriked(_carromBoard);
                        break;
                    case 6:
                        _action.RegisterAction(_currentPlayer, StrikeType.No_Strike);
                        SwitchPlayer();
                        break;

                }
                if (_action.CheckFoul(_currentPlayer))
                    _action.AddPoints(_currentPlayer, (int)StrikeType.Foul);
                if (_action.CheckConsecutiveNoStrike(_currentPlayer))
                    _action.AddPoints(_currentPlayer, (int)StrikeType.Consecutive_3_NoStrike);
            PrintScore();
        }

        private void PassStriker(StrikeType strikeType)
        {
            _action.RegisterAction(_currentPlayer, strikeType);
            _action.AddPoints(_currentPlayer, (int)strikeType);
            SwitchPlayer();
        }
    }
}
