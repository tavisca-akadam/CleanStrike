using CleanStrike.Interfaces;
using CleanStrike.Models;
using System;
using System.Linq;

namespace CleanStrike
{
    public class Game : IGame
    {
        private CarromBoard _carromBoard;
        private Player _player1;
        private Player _player2;
        private Player _striker;

        private int _foulCounter = 0;
        private int _noStrikeCounter = 0;
        public Game()
        {
            _carromBoard = new CarromBoard();
            _player1 = new Player("Anil", true);
            _player2 = new Player("Ak", false);
            Start();
        }

        public void Start()
        {
            if (_player1.IsPlayTurn)
            {
                _striker = _player1;
                _player1.ChnageTurn();
                _player2.ChnageTurn();
            }
            else
            {
                _striker = _player2;
                _player1.ChnageTurn();
                _player2.ChnageTurn();
            }
        }
        public void RegisterScore(int score)
        {
            _striker.Score += score;
        }

        public void CoinStriked()
        {
            if (_carromBoard.BlackCoins.Count != 0)
                _carromBoard.BlackCoins.Count--;
            else if (_carromBoard.RedCoins.Count != 0)
                _carromBoard.RedCoins.Count--;
            else
                throw new Exception();  //TODO: Write Custom Exceptions
        }

        public Player GetWinner()
        {
            if (((_player1.Score >= KeyStore.GameSettings.Min_Winining_Points) || (_player2.Score >= KeyStore.GameSettings.Min_Winining_Points))
                && Math.Abs(_player1.Score - _player2.Score) >= KeyStore.GameSettings.Min_Win_Point_Diff)
                return _player1.Score > _player2.Score ? _player1 : _player2;
            return null;
        }

        public bool IsConsecutiveNoStrike()
        {
            if(_noStrikeCounter == 3)
            {
                _noStrikeCounter = 0;
                return true;
            }
            return false;
        }

        public bool IsFoul()
        {
            var playingHistory = _striker.StrikeHistory;
            bool isFoul = false;
            int count = playingHistory.Count;
            int index = (count - KeyStore.GameSettings.Consecutive_Loosing_Limit);

            if (count >= KeyStore.GameSettings.Consecutive_Loosing_Limit)
            {
                isFoul = true;
                for(; index < count; index++)
                {
                    if (!IsLoosingPoint(playingHistory[index]))
                    {
                        isFoul = false;
                        break;
                    }
                }
            }
            return isFoul;
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

        bool IsLoosingPoint(StrikeType strikeType)
        {
            return ((strikeType == StrikeType.Consecutive_3_NoStrike) || (strikeType == StrikeType.Defunt_Coin) ||
                (strikeType == StrikeType.Striker_Strike));
        }

        public bool IsGameOver()
        {
            return (_carromBoard.BlackCoins.Count == 0 && _carromBoard.RedCoins.Count == 0);
        }

        public void Print()
        {
            Console.WriteLine($"Total black coins remaining : {_carromBoard.BlackCoins.Count}.");
            Console.WriteLine($"Total Red coins remaining : {_carromBoard.RedCoins.Count}.");
            Console.WriteLine($"Player 1: {_player1.Name }'s score  : {_player1.Score}.");
            Console.WriteLine($"Player 2: {_player2.Name }'s score  : {_player2.Score}.");
        }

        public void RegisterMove(StrikeType move)
        {
            _striker.StrikeHistory.Add(move);

            RegisterScore((int)move);

            if (move == StrikeType.Defunt_Coin || move == StrikeType.Striker_Strike)
                _foulCounter++;
            else if(move == StrikeType.No_Strike)
                _noStrikeCounter++;
            else
            {
                _foulCounter = 0;
                _noStrikeCounter = 0;
            }

        }

        public void SwitchPlayer()
        {
            if (_player1.IsPlayTurn)
            {
                _striker = _player1;
                _player1.ChnageTurn();
                _player2.ChnageTurn();
            }
            else
            {
                _striker = _player2;
                _player1.ChnageTurn();
                _player2.ChnageTurn();
            }
        }

        public void RedCoinStriked()
        {
            if (_carromBoard.RedCoins.Count != 0)
                _carromBoard.RedCoins.Count--;
            else
                throw new Exception();  //TODO: Write Custom Exceptions
        }

        public void StrikerStriked()
        {
            throw new NotImplementedException();
        }

        public void Play()
        {
            int caseType = -1;


            do
            {
                Console.WriteLine(_striker.Name + " Choose from the list");
                Console.WriteLine("1. STRIKE");
                Console.WriteLine("2. MultiSTRIKE");
                Console.WriteLine("3. Red STRIKE");
                Console.WriteLine("4. Striker STRIKE");
                Console.WriteLine("5. Defunct Coin");
                Console.WriteLine("6. None");
                Console.WriteLine("7. Exit");

                caseType = Convert.ToInt32(Console.ReadLine());
                Register(caseType);

                if (IsFoul())
                    RegisterScore((int)StrikeType.Foul);
                if (IsConsecutiveNoStrike())
                    RegisterScore((int)StrikeType.Consecutive_3_NoStrike);

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
                    RegisterMove(strikeType);
                    SwitchPlayer();
                    break;
                case StrikeType.Striker_Strike:
                    RegisterMove(strikeType);
                    SwitchPlayer();
                    break;
                case StrikeType.RedCoin_Strike:
                    RegisterMove(strikeType);
                    RedCoinStriked();
                    SwitchPlayer();
                    break;
                case StrikeType.Strike:
                    RegisterMove(strikeType);
                    SwitchPlayer();
                    break;
                case StrikeType.Multi_Strike:
                    RegisterMove(strikeType);
                    SwitchPlayer();
                    break;
                case StrikeType.Defunt_Coin:
                    RegisterMove(strikeType);
                    CoinStriked();
                    SwitchPlayer();
                    break;
            }

            
        }
    }
}
