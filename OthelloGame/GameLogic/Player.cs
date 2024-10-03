using System;
using System.Collections.Generic;

namespace Othello
{
    class Player
    {
        private bool m_IsMyTurn = false;
        private string m_Name;
        private char m_Char;
        private int m_Score = 2;
        private bool m_IsComputer = false;
        private static readonly Random random = new Random();

        public Player(string i_PlayerName, char i_PlayerDisc, bool i_IsComputer = false)
        {
            m_Name = i_PlayerName;
            m_Char = i_PlayerDisc;
            m_IsComputer = i_IsComputer;
            if (m_IsComputer) m_Name = "Computer";
        }

        public string PlayerName
        {
            get { return m_Name; }
        }
        public char PlayerDisc
        {
            get { return m_Char; }
        }
        public int PlayerScore
        {
            get { return m_Score; }
            set { m_Score = value; }
        }
        public bool IsComputer
        {
            get { return m_IsComputer; }
        }
        public bool IsMyTurn
        {
            get { return m_IsMyTurn; }
            set { m_IsMyTurn = value; }
        }

        public (int rowIndex, int colIndex) GetMove(string i_UserCellChoice = "", List<(int, int)> i_ValidMoves = null)
        {
            (int rowIndex, int colIndex) move = (-1, -1);

            if (this.IsComputer)
            {
                move = getComputerMove(i_ValidMoves);
            }
            else
            {
                if (i_UserCellChoice.Length >= 2 && Char.IsLetter(i_UserCellChoice, 0) && int.TryParse(i_UserCellChoice.Substring(1), out int row))
                {
                    move = getHumanMove(i_UserCellChoice);
                }
            }

            return move;
        }

        private (int rowIndex, int colIndex) getComputerMove(List<(int, int)> i_ValidMoves)
        {
            (int rowIndex, int colIndex) move = (-1, -1);

            if (i_ValidMoves.Count > 0)
            {
                move = i_ValidMoves[random.Next(i_ValidMoves.Count)];
            }

            return move;
        }

        private (int rowIndex, int colIndex) getHumanMove(string i_UserCellChoice)
        {
            char columnLetter = char.ToUpper(i_UserCellChoice[0]);
            Board.eBoardColumn columnEnum;
            Enum.TryParse(columnLetter.ToString(), out columnEnum);
            int colIndex = (int)columnEnum;
            int rowIndex = int.Parse(i_UserCellChoice.Substring(1)) - 1;

            return (rowIndex, colIndex);
        }
    }
}
