using System;
using System.Collections.Generic;

namespace OthelloWinForms
{
    public class GameManager
    {
        private readonly Board r_Board;
        private readonly Player r_Player1;
        private readonly Player r_Player2;
        private Player m_CurrentPlayer;
        private bool m_GameOver = false;
        public event Action<string> NoValidMoves;
        public event Action<Player> GameOverEvent;

        public GameManager(int i_BoardSize, bool i_IsAgainstComputer)
        {
            r_Board = new Board(i_BoardSize);
            r_Player1 = new Player("Yellow", 'O');
            r_Player2 = new Player("Red", 'X', i_IsAgainstComputer);
            m_CurrentPlayer = r_Player1;
            m_CurrentPlayer.IsMyTurn = true;
        }

        public Board GameBoard
        {
            get => r_Board;
        }

        public Player Player1
        {
            get => r_Player1;
        }

        public Player Player2
        {
            get => r_Player2;
        }

        public Player CurrentPlayer
        {
            get => m_CurrentPlayer;
        }

        public bool IsGameOver 
        {
            get => m_GameOver;
        }

        public List<(int, int)> GetValidMoves(Player i_Player)
        {
            List<(int, int)> validMoves = new List<(int, int)>();

            for (int row = 0; row < r_Board.Size; row++)
            {
                for (int col = 0; col < r_Board.Size; col++)
                {
                    if (IsValidMove(row, col, i_Player))
                    {
                        validMoves.Add((row, col));
                    }
                }
            }

            return validMoves;
        }

        public bool IsValidMove(int i_Row, int i_Col, Player i_Player)
        {
            bool isValid= false;
            char opponentDisc = i_Player.Disc == 'O' ? 'X' : 'O';

            for (int rowDirection = -1; rowDirection <= 1; rowDirection++)
            {
                for (int colDirection = -1; colDirection <= 1; colDirection++)
                {
                    if (rowDirection != 0 || colDirection != 0)
                    {
                        if (checkDirection(i_Row, i_Col, rowDirection, colDirection, i_Player.Disc, opponentDisc, false))
                        {
                            isValid = true;
                        }
                    }
                }
            }

            if (!r_Board.IsInBounds(i_Row, i_Col) || r_Board.BoardArray[i_Row, i_Col] != '*')
            {
                isValid = false;
            }

            return isValid;
        }

        private bool checkDirection(int i_Row,
                                    int i_Col,
                                    int i_RowDirection,
                                    int i_ColDirection,
                                    char i_PlayerDisc,
                                    char i_OpponentDisc,
                                    bool i_ShouldFlip)
        {
            int row = i_Row + i_RowDirection;
            int col = i_Col + i_ColDirection;
            bool hasOpponentBetween = false;
            bool result = false;

            while (r_Board.IsInBounds(row, col))
            {
                char currentDisc = r_Board.BoardArray[row, col];

                if (currentDisc == i_OpponentDisc)
                {
                    hasOpponentBetween = true;
                }
                else if (currentDisc == i_PlayerDisc)
                {
                    if (hasOpponentBetween)
                    {
                        if (i_ShouldFlip)
                        {
                            flipDiscsInDirection(i_Row, i_Col, i_RowDirection, i_ColDirection, i_PlayerDisc);
                        }
                        result = true;
                    }

                    break;
                }
                else
                {
                    break;
                }

                row += i_RowDirection;
                col += i_ColDirection;
            }

            return result;
        }

        private void flipDiscsInDirection(int i_Row, int i_Col,
                                          int i_RowDirection,
                                          int i_ColDirection,
                                          char i_PlayerDisc)
        {
            int row = i_Row + i_RowDirection;
            int col = i_Col + i_ColDirection;

            while (r_Board.BoardArray[row, col] != i_PlayerDisc)
            {
                r_Board.PlaceDisc(row, col, i_PlayerDisc);
                updateScores(i_PlayerDisc);
                row += i_RowDirection;
                col += i_ColDirection;
            }
        }

        private void updateScores(char i_PlayerDisc)
        {
            if (i_PlayerDisc == r_Player1.Disc)
            {
                r_Player1.Score++;
                r_Player2.Score--;
            }
            else
            {
                r_Player1.Score--;
                r_Player2.Score++;
            }
        }

        public void MakeMove(int i_Row, int i_Col)
        {
            char opponentDisc = m_CurrentPlayer.Disc == 'O' ? 'X' : 'O';
            bool validMove = false;

            for (int rowDirection = -1; rowDirection <= 1; rowDirection++)
            {
                for (int colDirection = -1; colDirection <= 1; colDirection++)
                {
                    if (rowDirection != 0 || colDirection != 0)
                    {
                        if (checkDirection(i_Row, i_Col, rowDirection, colDirection, m_CurrentPlayer.Disc, opponentDisc, true))
                        {
                            validMove = true;
                        }
                    }
                }
            }

            if (validMove)
            {
                r_Board.PlaceDisc(i_Row, i_Col, m_CurrentPlayer.Disc);
                m_CurrentPlayer.Score++;
            }
        }

        public void SwitchTurns()
        {
            Player otherPlayer = m_CurrentPlayer == r_Player1 ? r_Player2 : r_Player1;

            if (GetValidMoves(otherPlayer).Count > 0)
            {
                m_CurrentPlayer.IsMyTurn = false;
                m_CurrentPlayer = otherPlayer;
                m_CurrentPlayer.IsMyTurn = true;
            }
            else if (GetValidMoves(m_CurrentPlayer).Count > 0)
            {
                NoValidMoves?.Invoke($"{otherPlayer.Name} has no valid moves. {m_CurrentPlayer.Name} plays again.");
            }
            else
            {
                m_GameOver = true;
                GameOverEvent?.Invoke(GetWinner());

            }
        }

        public Player GetWinner()
        {
            Player winner = null;

            if (r_Player1.Score > r_Player2.Score)
            {
                winner = r_Player1;
            }
            else if (r_Player2.Score > r_Player1.Score)
            {
                winner = r_Player2;
            }

            return winner;
        }

        public void ResetGame()
        {
            r_Board.InitializeBoard();
            r_Player1.Score = 2;
            r_Player2.Score = 2;
            m_CurrentPlayer = r_Player1;
            m_CurrentPlayer.IsMyTurn = true;
            m_GameOver = false;
        }
    }
}
