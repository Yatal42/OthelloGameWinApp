using OthelloWinForms.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace OthelloWinForms
{
    public partial class FormGame : Form
    {
        private readonly GameManager r_GameManager;
        private readonly PictureBox[,] r_BoardPictureBoxes;

        public FormGame(int i_boardSize, bool i_isAgainstComputer)
        {
            InitializeComponent();
            r_GameManager = new GameManager(i_boardSize, i_isAgainstComputer);
            r_BoardPictureBoxes = new PictureBox[i_boardSize, i_boardSize];

            initializeBoardUI(i_boardSize);
            r_GameManager.GameBoard.DiscPlaced += onDiscPlaced;
            updateBoardDisplay();

            if (r_GameManager.CurrentPlayer.IsComputer)
            {
                computerTurn();
            }
        }

        private void initializeBoardUI(int i_boardSize)
        {
            int tileSize = 50;
            int boardWidth = i_boardSize * tileSize;
            int boardHeight = i_boardSize * tileSize;
            this.ClientSize = new Size(boardWidth + 20, boardHeight + 40);
            this.StartPosition = FormStartPosition.CenterScreen;

            for (int row = 0; row < i_boardSize; row++)
            {
                for (int col = 0; col < i_boardSize; col++)
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
                    pictureBox.MouseEnter += pictureBox_MouseEnter; 
                    pictureBox.MouseLeave += pictureBox_MouseLeave; 

                    r_BoardPictureBoxes[row, col] = pictureBox;
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

            if (r_GameManager.IsValidMove(row, col, r_GameManager.CurrentPlayer))
            {
                pictureBox.BorderStyle = BorderStyle.Fixed3D; 
            }
        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
        }

        private void boardPictureBox_Click(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = sender as PictureBox;
            Point location = (Point)clickedPictureBox.Tag;
            int row = location.X;
            int col = location.Y;

            if (r_GameManager.IsValidMove(row, col, r_GameManager.CurrentPlayer))
            {
                r_GameManager.MakeMove(row, col);
                updateBoardDisplay();
                r_GameManager.SwitchTurns();

                if (r_GameManager.IsGameOver)
                {
                    declareWinner();
                }
                else if (r_GameManager.CurrentPlayer.IsComputer)
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
            char[,] boardArray = r_GameManager.GameBoard.BoardArray;
            int boardSize = boardArray.GetLength(0);
            Player currentPlayer = r_GameManager.CurrentPlayer;

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    char disc = boardArray[row, col];
                    PictureBox pictureBox = r_BoardPictureBoxes[row, col];

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

                        if (r_GameManager.IsValidMove(row, col, currentPlayer))
                        {
                            pictureBox.Cursor = Cursors.Hand;
                            pictureBox.BackColor = Color.MediumSpringGreen;
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

        private void onDiscPlaced(int i_rowIndex, int i_colIndex, char i_playerDisc)
        {
            PictureBox pictureBox = r_BoardPictureBoxes[i_rowIndex, i_colIndex];

            if (i_playerDisc == 'O')
            {
                pictureBox.Image = Properties.Resources.CoinYellow;
            }
            else if (i_playerDisc == 'X')
            {
                pictureBox.Image = Properties.Resources.CoinRed;
            }

            pictureBox.Cursor = Cursors.Default;
        }

        private void declareWinner()
        {
            Player winner = r_GameManager.GetWinner();
            int blackScore = r_GameManager.Player1.Score;
            int whiteScore = r_GameManager.Player2.Score;
            string message;

            if (winner != null)
            {
                message = $"{winner.Name} wins! with {winner.Score}" + Environment.NewLine + $"Final score is {blackScore}|{whiteScore}";

            }
            else
            {
                message = $"It's a tie!" + Environment.NewLine + $"Final score is {blackScore}|{whiteScore}";
            }

            DialogResult result = MessageBox.Show($"{message}" + Environment.NewLine + "Would you like to play again?", "Othello", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                r_GameManager.ResetGame();
                updateBoardDisplay();

                if (r_GameManager.CurrentPlayer.IsComputer)
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

            Player computerPlayer = r_GameManager.CurrentPlayer;
            List<(int, int)> validMoves = r_GameManager.GetValidMoves(computerPlayer);

            if (validMoves.Count == 0)
            {
                MessageBox.Show("Computer has no valid moves.", "No Valid Moves", MessageBoxButtons.OK, MessageBoxIcon.Information);
                r_GameManager.SwitchTurns();
                updateBoardDisplay();

                if (r_GameManager.IsGameOver)
                {
                    declareWinner();
                }
                else if (r_GameManager.CurrentPlayer.IsComputer)
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

            r_GameManager.MakeMove(move.rowIndex, move.colIndex);
            updateBoardDisplay();
            r_GameManager.SwitchTurns();

            if (r_GameManager.IsGameOver)
            {
                declareWinner();
            }
            else if (r_GameManager.CurrentPlayer.IsComputer)
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
