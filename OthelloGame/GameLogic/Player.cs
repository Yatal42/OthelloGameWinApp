using System;
using System.Collections.Generic;

namespace OthelloWinForms
{
    public class Player
    {
        private string m_Name;
        private char m_Disc;
        private int m_Score;
        private bool m_IsMyTurn;
        private bool m_IsComputer;
        private static readonly Random sr_Random = new Random();

        public Player(string i_Name, char i_Disc, bool i_IsComputer = false)
        {
            m_Name = i_IsComputer ? "Computer" : i_Name;
            m_Disc = i_Disc;
            m_Score = 2;
            m_IsMyTurn = false;
            m_IsComputer = i_IsComputer;
        }

        public string Name
        {
            get => m_Name;
        }

        public char Disc
        {
            get => m_Disc;
        }

        public int Score
        {
            get => m_Score;
            set => m_Score = value;
        }

        public bool IsMyTurn
        {
            get => m_IsMyTurn;
            set => m_IsMyTurn = value;
        }

        public bool IsComputer
        {
            get => m_IsComputer;
        }

        public (int rowIndex, int colIndex) GetMove(List<(int, int)> i_ValidMoves)
        {
            return (m_IsComputer && i_ValidMoves.Count > 0)
                ? i_ValidMoves[sr_Random.Next(i_ValidMoves.Count)]
                : (-1, -1);
        }

    }
}
