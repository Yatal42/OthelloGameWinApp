using System.Collections.Generic;
using System.Windows.Forms;

namespace OthelloWinForms
{
    public class GameManager
    {
        private readonly Board r_Board;
        private readonly Player r_Player1;
        private readonly Player r_Player2;
        private Player m_CurrentPlayer;
        private bool m_GameOver = false;

        public GameManager(int i_BoardSize, bool i_IsAgainstComputer)
        {
            r_Board = new Board(i_BoardSize);
            r_Player1 = new Player("Yellow", 'O');
            r_Player2 = new Player("Red", 'X', i_IsAgainstComputer);

            m_CurrentPlayer = r_Player1;
            m_CurrentPlayer.IsMyTurn = true;
        }

        public Board GameBoard => r_Board;
        public Player Player1 => r_Player1;
        public Player Player2 => r_Player2;
        public Player CurrentPlayer => m_CurrentPlayer;
        public bool IsGameOver => m_GameOver;

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
            if (!r_Board.IsInBounds(i_Row, i_Col) || r_Board.BoardArray[i_Row, i_Col] != '*')
            {
                return false;
            }

            char opponentDisc = i_Player.Disc == 'O' ? 'X' : 'O';

            for (int deltaRow = -1; deltaRow <= 1; deltaRow++)
            {
                for (int deltaCol = -1; deltaCol <= 1; deltaCol++)
                {
                    if (deltaRow != 0 || deltaCol != 0)
                    {
                        if (CheckDirection(i_Row, i_Col, deltaRow, deltaCol, i_Player.Disc, opponentDisc, false))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool CheckDirection(int i_Row, int i_Col, int i_DeltaRow, int i_DeltaCol, char i_PlayerDisc, char i_OpponentDisc, bool i_ShouldFlip)
        {
            int row = i_Row + i_DeltaRow;
            int col = i_Col + i_DeltaCol;
            bool hasOpponentBetween = false;

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
                            FlipDiscsInDirection(i_Row, i_Col, i_DeltaRow, i_DeltaCol, i_PlayerDisc);
                        }
                        return true;
                    }
                    break;
                }
                else
                {
                    break;
                }

                row += i_DeltaRow;
                col += i_DeltaCol;
            }

            return false;
        }

        private void FlipDiscsInDirection(int i_Row, int i_Col, int i_DeltaRow, int i_DeltaCol, char i_PlayerDisc)
        {
            int row = i_Row + i_DeltaRow;
            int col = i_Col + i_DeltaCol;

            while (r_Board.BoardArray[row, col] != i_PlayerDisc)
            {
                r_Board.PlaceDisc(row, col, i_PlayerDisc);
                UpdateScores(i_PlayerDisc);
                row += i_DeltaRow;
                col += i_DeltaCol;
            }
        }

        private void UpdateScores(char i_PlayerDisc)
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

            for (int deltaRow = -1; deltaRow <= 1; deltaRow++)
            {
                for (int deltaCol = -1; deltaCol <= 1; deltaCol++)
                {
                    if (deltaRow != 0 || deltaCol != 0)
                    {
                        if (CheckDirection(i_Row, i_Col, deltaRow, deltaCol, m_CurrentPlayer.Disc, opponentDisc, true))
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
                MessageBox.Show($"{otherPlayer.Name} has no valid moves. {m_CurrentPlayer.Name} plays again.", "No Valid Moves", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                m_GameOver = true;
            }
        }

        public Player GetWinner()
        {
            if (r_Player1.Score > r_Player2.Score)
            {
                return r_Player1;
            }
            else if (r_Player2.Score > r_Player1.Score)
            {
                return r_Player2;
            }
            else
            {
                return null; 
            }
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
