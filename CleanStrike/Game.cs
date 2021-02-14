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

        public void AddPlayer(Player player)
        {
            throw new NotImplementedException();
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
            Console.WriteLine("Match Draw...");
        }

        public void SwitchPlayer()
        {
            _currentPlayer = _playerQueues.Dequeue();
            _playerQueues.Enqueue(_currentPlayer);
        }

        public void PlayGame()
        {
            int caseType = -1;


            do
            {
                Console.WriteLine(_currentPlayer.Name + " Choose from the list");
                Console.WriteLine("1. STRIKE");
                Console.WriteLine("2. MultiSTRIKE");
                Console.WriteLine("3. Red STRIKE");
                Console.WriteLine("4. Striker STRIKE");
                Console.WriteLine("5. Defunct Coin");
                Console.WriteLine("6. None");
                Console.WriteLine("7. Exit");

                caseType = Convert.ToInt32(Console.ReadLine());
                Options((StrikeType)caseType);

                if (_action.CheckFoul(_currentPlayer))
                    _action.AddPoints(_currentPlayer, (int)StrikeType.Foul);
                if (_action.CheckConsecutiveNoStrike(_currentPlayer))
                    _action.AddPoints(_currentPlayer, (int)StrikeType.Consecutive_3_NoStrike);

                if (GetWinner() != null)
                    caseType = 7;

            } while (!(IsGameOver() || caseType == 7));
            if (IsGameDraw())
                Console.WriteLine("Game DRAW!!...");
            PrintScore();
        }

        private void Options(StrikeType strikeType)
        {
            switch (strikeType)
            {
                case StrikeType.No_Strike:
                    _action.RegisterAction(_currentPlayer, strikeType);
                    SwitchPlayer();
                    break;
                case StrikeType.Striker_Strike:
                    PassStriker(strikeType);
                    break;
                case StrikeType.RedCoin_Strike:
                    PassStriker(strikeType);
                    _action.OnRedCoinPocketed(_carromBoard);
                    break;
                case StrikeType.Strike:
                    PassStriker(strikeType);
                    break;
                case StrikeType.Multi_Strike:
                    PassStriker(strikeType);
                    break;
                case StrikeType.Defunt_Coin:
                    PassStriker(strikeType);
                    _action.OnCoinStriked(_carromBoard);
                    break;
                
            }
            
        }

        private void PassStriker(StrikeType strikeType)
        {
            _action.RegisterAction(_currentPlayer, strikeType);
            _action.AddPoints(_currentPlayer, (int)strikeType);
            SwitchPlayer();
        }
    }
}
