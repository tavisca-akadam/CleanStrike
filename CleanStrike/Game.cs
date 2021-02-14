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

        private Queue<Player> _playerQueue = new Queue<Player>();
        private IAction _action;
        private int _foulCounter = 0;
        private int _noStrikeCounter = 0;
        public Game(int blackCoins, int redCoins)
        {
            _carromBoard = new CarromBoard(blackCoins, redCoins);
            _player1 = new Player("Anil", true);
            _player2 = new Player("Ak", false);
            _action = new GameAction();
            InitBoard();
        }



        public Player GetWinner()
        {
            if (((_player1.Score >= KeyStore.GameSettings.Min_Winining_Points) || (_player2.Score >= KeyStore.GameSettings.Min_Winining_Points))
                && Math.Abs(_player1.Score - _player2.Score) >= KeyStore.GameSettings.Min_Win_Point_Diff)
                return _player1.Score > _player2.Score ? _player1 : _player2;
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



        public bool IsGameOver()
        {
            return (_carromBoard.BlackCoins == 0 && _carromBoard.RedCoins == 0);
        }

        public void Print()
        {
            Console.WriteLine($"Total black coins remaining : {_carromBoard.BlackCoins}.");
            Console.WriteLine($"Total Red coins remaining : {_carromBoard.RedCoins}.");
            Console.WriteLine($"Player 1: {_player1.Name }'s score  : {_player1.Score}.");
            Console.WriteLine($"Player 2: {_player2.Name }'s score  : {_player2.Score}.");
        }

 

        public void SwitchPlayer()
        {
            _currentPlayer = _playerQueue.Dequeue();
            _playerQueue.Enqueue(_currentPlayer);
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
                Register(caseType);

                if (_action.CheckFoul(_currentPlayer))
                    _action.AddPoints(_currentPlayer, (int)StrikeType.Foul);
                if (_action.CheckConsecutiveNoStrike(_currentPlayer))
                    _action.AddPoints(_currentPlayer, (int)StrikeType.Consecutive_3_NoStrike);

                if (GetWinner() != null)
                    caseType = 7;

                Print();
            } while (!(IsGameOver() || caseType == 7));
            if (IsGameDraw())
                Console.WriteLine("Game DRAW!!...");
            Print();
        }

        private void Register(int caseType)
        {
               switch (caseType)
            {
                case 1:
                    Strikes(StrikeType.Strike);
                    break;
                case 2:
                    Strikes(StrikeType.Multi_Strike);
                    break;
                case 3:
                    Strikes(StrikeType.RedCoin_Strike);
                    break;
                case 4:
                    Strikes(StrikeType.Striker_Strike);
                    break;
                case 5:
                    Strikes(StrikeType.Defunt_Coin);
                    break;
                case 6:
                    Strikes(StrikeType.No_Strike);
                    break;
            }
        }

        private void Strikes(StrikeType strikeType)
        {
            switch (strikeType)
            {
                case StrikeType.No_Strike:
                    _action.RegisterAction(_currentPlayer, strikeType);
                    SwitchPlayer();
                    break;
                case StrikeType.Striker_Strike:
                    _action.RegisterAction(_currentPlayer, strikeType);
                    _action.AddPoints(_currentPlayer, (int)strikeType);
                    SwitchPlayer();
                    break;
                case StrikeType.RedCoin_Strike:
                    _action.RegisterAction(_currentPlayer, strikeType);
                    _action.AddPoints(_currentPlayer, (int)strikeType);
                    _action.OnRedCoinPocketed(_carromBoard);
                    SwitchPlayer();
                    break;
                case StrikeType.Strike:
                    _action.RegisterAction(_currentPlayer, strikeType);
                    _action.AddPoints(_currentPlayer, (int)strikeType);
                    SwitchPlayer();
                    break;
                case StrikeType.Multi_Strike:
                    _action.RegisterAction(_currentPlayer, strikeType);
                    _action.AddPoints(_currentPlayer, (int)strikeType);
                    SwitchPlayer();
                    break;
                case StrikeType.Defunt_Coin:
                    _action.RegisterAction(_currentPlayer, strikeType);
                    _action.AddPoints(_currentPlayer, (int)strikeType);
                    _action.OnCoinStriked(_carromBoard);
                    SwitchPlayer();
                    break;
                
            }

            
        }

        public void InitBoard()
        {
            _playerQueue.Enqueue(_player2);
            _playerQueue.Enqueue(_player1);
            _currentPlayer = _player1;
        }

        public void AddPlayer(Player player)
        {
            throw new NotImplementedException();
        }

    }
}
