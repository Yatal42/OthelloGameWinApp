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
        private Timer m_ComputerMoveTimer;

        public FormGame(int i_BoardSize, bool i_IsAgainstComputer)
        {
            InitializeComponent();
            r_GameManager = new GameManager(i_BoardSize, i_IsAgainstComputer);
            r_BoardPictureBoxes = new PictureBox[i_BoardSize, i_BoardSize];
            initializeBoardUI(i_BoardSize);

            r_GameManager.GameBoard.DiscPlaced += onDiscPlaced;
            r_GameManager.NoValidMoves += handleNoValidMoves;
            r_GameManager.GameOverEvent += handleGameOver;
            m_ComputerMoveTimer = new Timer();
            m_ComputerMoveTimer.Interval = 700; 
            m_ComputerMoveTimer.Tick += ComputerMoveTimer_Tick;
            updateBoardDisplay();

            if (r_GameManager.CurrentPlayer.IsComputer)
            {
                computerTurn();
            }
        }

        private void initializeBoardUI(int i_BoardSize)
        {
            int tileSize = 50;
            int boardWidth = i_BoardSize * tileSize;
            int boardHeight = i_BoardSize * tileSize;
            this.ClientSize = new Size(boardWidth + 20, boardHeight + 40);
            this.StartPosition = FormStartPosition.CenterScreen;

            for (int row = 0; row < i_BoardSize; row++)
            {
                for (int col = 0; col < i_BoardSize; col++)
                {
                    PictureBox pictureBox = new PictureBox
                    {
                        Size = new Size(tileSize, tileSize),
                        Location = new Point(10 + col * tileSize, 10 + row * tileSize),
                        Tag = new Point(row, col),
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.White,
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

        private void handleNoValidMoves(string message)
        {
            MessageBox.Show(message, "No Valid Moves", MessageBoxButtons.OK, MessageBoxIcon.Information);
            updateBoardDisplay();

            if (r_GameManager.CurrentPlayer.IsComputer)
            {
                computerTurn();
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
                processMove();
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

        private void onDiscPlaced(int i_RowIndex, int i_ColIndex, char i_PlayerDisc)
        {
            PictureBox pictureBox = r_BoardPictureBoxes[i_RowIndex, i_ColIndex];

            if (i_PlayerDisc == 'O')
            {
                pictureBox.Image = Resources.CoinYellow;
            }
            else if (i_PlayerDisc == 'X')
            {
                pictureBox.Image = Resources.CoinRed;
            }

            pictureBox.Cursor = Cursors.Default;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
        }

        private void handleGameOver(Player winner)
        {
            int player1Score = r_GameManager.Player1.Score;
            int player2Score = r_GameManager.Player2.Score;
            string message;

            if (winner != null)
            {
                message = $"{winner.Name} wins with {winner.Score} points!" + Environment.NewLine +
                          $"Final score is {r_GameManager.Player1.Name}: {player1Score} | {r_GameManager.Player2.Name}: {player2Score}";
            }
            else
            {
                message = $"It's a tie!" + Environment.NewLine +
                          $"Final score is {r_GameManager.Player1.Name}: {player1Score} | {r_GameManager.Player2.Name}: {player2Score}";
            }

            DialogResult result = MessageBox.Show($"{message}" + Environment.NewLine + "Would you like to play again?", "Othello", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                r_GameManager.ResetGame();
                updateBoardDisplay();
            }
            else
            {
                Application.Exit(); 
            }
        }

        private void processMove()
        {
            updateBoardDisplay();
            r_GameManager.SwitchTurns();
            updateBoardDisplay();

            if (r_GameManager.IsGameOver)
            {
                handleGameOver(r_GameManager.GetWinner());
            }
            else if (r_GameManager.CurrentPlayer.IsComputer)
            {
                computerTurn();
            }
        }

        private void computerTurn()
        {
            m_ComputerMoveTimer.Start();
        }

        private void ComputerMoveTimer_Tick(object sender, EventArgs e)
        {
            m_ComputerMoveTimer.Stop();
            Player computerPlayer = r_GameManager.CurrentPlayer;
            List<(int, int)> validMoves = r_GameManager.GetValidMoves(computerPlayer);

            if (validMoves.Count == 0)
            {
                processMove();

                return;
            }

            var move = computerPlayer.GetMove(validMoves);
            r_GameManager.MakeMove(move.rowIndex, move.colIndex);
            processMove();
        }
    }
}