using OthelloWinForms.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace OthelloWinForms
{
    public partial class FormGame : Form
    {
        private readonly GameManager m_GameManager;
        private readonly PictureBox[,] m_BoardPictureBoxes;

        public FormGame(int boardSize, bool isAgainstComputer)
        {
            InitializeComponent();
            m_GameManager = new GameManager(boardSize, isAgainstComputer);
            m_BoardPictureBoxes = new PictureBox[boardSize, boardSize];
            initializeBoardUI(boardSize);

            // Subscribe to the DiscPlaced event
            m_GameManager.GameBoard.DiscPlaced += onDiscPlaced;

            // Update the display according to the initial state
            updateBoardDisplay();

            if (m_GameManager.CurrentPlayer.IsComputer)
            {
                computerTurn();
            }
        }

        private void initializeBoardUI(int boardSize)
        {
            int tileSize = 50;
            int boardWidth = boardSize * tileSize;
            int boardHeight = boardSize * tileSize;
            this.ClientSize = new Size(boardWidth + 20, boardHeight + 40);
            this.StartPosition = FormStartPosition.CenterScreen;

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    PictureBox pictureBox = new PictureBox
                    {
                        Size = new Size(tileSize, tileSize),
                        Location = new Point(10 + col * tileSize, 10 + row * tileSize),
                        Tag = new Point(row, col),
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.Green,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                    };
                    pictureBox.Click += boardPictureBox_Click;
                    pictureBox.MouseEnter += pictureBox_MouseEnter; // Add MouseEnter event handler
                    pictureBox.MouseLeave += pictureBox_MouseLeave; // Add MouseLeave event handler
                    m_BoardPictureBoxes[row, col] = pictureBox;
                    this.Controls.Add(pictureBox);
                }
            }
        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            Point location = (Point)pictureBox.Tag;
            int row = location.X;
            int col = location.Y;

            // Check if it's a valid move for the current player
            if (m_GameManager.IsValidMove(row, col, m_GameManager.CurrentPlayer))
            {
                pictureBox.BorderStyle = BorderStyle.Fixed3D; // Change border style to indicate hover
            }
        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;

            // Revert to default border style
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
        }


        private void boardPictureBox_Click(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = sender as PictureBox;
            Point location = (Point)clickedPictureBox.Tag;
            int row = location.X;
            int col = location.Y;

            if (m_GameManager.IsValidMove(row, col, m_GameManager.CurrentPlayer))
            {
                m_GameManager.MakeMove(row, col);
                updateBoardDisplay();
                m_GameManager.SwitchTurns();

                if (m_GameManager.IsGameOver)
                {
                    declareWinner();
                }
                else if (m_GameManager.CurrentPlayer.IsComputer)
                {
                    computerTurn();
                }
                else
                {
                    updateBoardDisplay();
                }
            }
            else
            {
                MessageBox.Show("Invalid move! Please select a valid cell.", "Invalid Move", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void updateBoardDisplay()
        {
            char[,] boardArray = m_GameManager.GameBoard.BoardArray;
            int boardSize = boardArray.GetLength(0);
            Player currentPlayer = m_GameManager.CurrentPlayer;

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    char disc = boardArray[row, col];
                    PictureBox pictureBox = m_BoardPictureBoxes[row, col];

                    if (disc == 'O')
                    {
                        pictureBox.Image = Resources.CoinYellow;
                        pictureBox.Cursor = Cursors.Default;
                        pictureBox.BackColor = Color.White;
                        pictureBox.BorderStyle = BorderStyle.FixedSingle;
                    }
                    else if (disc == 'X')
                    {
                        pictureBox.Image = Resources.CoinRed;
                        pictureBox.Cursor = Cursors.Default;
                        pictureBox.BackColor = Color.White;
                        pictureBox.BorderStyle = BorderStyle.FixedSingle;
                    }
                    else
                    {
                        pictureBox.Image = null;

                        if (m_GameManager.IsValidMove(row, col, currentPlayer))
                        {
                            pictureBox.Cursor = Cursors.Hand;
                            pictureBox.BackColor = Color.MediumSpringGreen;
                            // The border will be handled in the MouseEnter and MouseLeave events
                        }
                        else
                        {
                            pictureBox.Cursor = Cursors.Default;
                            pictureBox.BackColor = Color.White;
                            pictureBox.BorderStyle = BorderStyle.FixedSingle;
                        }
                    }
                }
            }

            this.Text = $"Othello - {currentPlayer.Name}'s Turn";
        }

        private void onDiscPlaced(int rowIndex, int colIndex, char playerDisc)
        {
            PictureBox pictureBox = m_BoardPictureBoxes[rowIndex, colIndex];

            if (playerDisc == 'O')
            {
                pictureBox.Image = Properties.Resources.CoinYellow;
            }
            else if (playerDisc == 'X')
            {
                pictureBox.Image = Properties.Resources.CoinRed;
            }

            pictureBox.Cursor = Cursors.Default;
        }

        private void declareWinner()
        {
            Player winner = m_GameManager.GetWinner();
            int blackScore = m_GameManager.Player1.Score;
            int whiteScore = m_GameManager.Player2.Score;
            string message;

            if (winner != null)
            {
                message = $"{winner.Name} wins!\nFinal score is {blackScore}/{whiteScore}";
            }
            else
            {
                message = $"It's a tie!\nFinal score is {blackScore}/{whiteScore}";
            }

            DialogResult result = MessageBox.Show($"{message}\nWould you like to play again?", "Othello", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                m_GameManager.ResetGame();
                updateBoardDisplay();

                if (m_GameManager.CurrentPlayer.IsComputer)
                {
                    computerTurn();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void computerTurn()
        {
            Application.DoEvents();
            System.Threading.Thread.Sleep(500);

            Player computerPlayer = m_GameManager.CurrentPlayer;
            List<(int, int)> validMoves = m_GameManager.GetValidMoves(computerPlayer);

            if (validMoves.Count == 0)
            {
                MessageBox.Show("Computer has no valid moves.", "No Valid Moves", MessageBoxButtons.OK, MessageBoxIcon.Information);
                m_GameManager.SwitchTurns();
                updateBoardDisplay();

                if (m_GameManager.IsGameOver)
                {
                    declareWinner();
                }
                else if (m_GameManager.CurrentPlayer.IsComputer)
                {
                    computerTurn();
                }
                else
                {
                    updateBoardDisplay();
                }

                return;
            }

            var move = computerPlayer.GetMove(validMoves);

            m_GameManager.MakeMove(move.rowIndex, move.colIndex);
            updateBoardDisplay();
            m_GameManager.SwitchTurns();

            if (m_GameManager.IsGameOver)
            {
                declareWinner();
            }
            else if (m_GameManager.CurrentPlayer.IsComputer)
            {
                computerTurn();
            }
            else
            {
                updateBoardDisplay();
            }
        }
    }
}
