using System;

namespace OthelloWinForms
{
    public class Board
    {
        private readonly int r_Size;
        private readonly char[,] r_BoardArray;
        public event Action<int, int, char> DiscPlaced;

        public Board(int i_Size)
        {
            r_Size = i_Size;
            r_BoardArray = new char[r_Size, r_Size];
            InitializeBoard();
        }

        public int Size
        {
            get => r_Size;
        }

        public char[,] BoardArray
        {     
            get  => r_BoardArray;
        }

        public void InitializeBoard()
        {
            for (int row = 0; row < r_Size; row++)
            {
                for (int col = 0; col < r_Size; col++)
                {
                    r_BoardArray[row, col] = '*';
                }
            }

            setStartingPosition();
        }

        private void setStartingPosition()
        {
            int mid = r_Size / 2;
            PlaceDisc(mid - 1, mid - 1, 'X');
            PlaceDisc(mid - 1, mid, 'O');
            PlaceDisc(mid, mid - 1, 'O');
            PlaceDisc(mid, mid, 'X');
        }

        public void PlaceDisc(int i_Row, int i_Col, char i_Disc)
        {
            if (IsInBounds(i_Row, i_Col))
            {
                r_BoardArray[i_Row, i_Col] = i_Disc;
                DiscPlaced?.Invoke(i_Row, i_Col, i_Disc);
            }
        }

        public bool IsInBounds(int i_Row, int i_Col)
        {
            return i_Row >= 0 && i_Row < r_Size && i_Col >= 0 && i_Col < r_Size;
        }
    }
}
