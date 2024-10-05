using System;
using System.Windows.Forms;

namespace OthelloWinForms
{
    public partial class FormSettings : Form
    {
        private int m_BoardSize = 6;
        public int BoardSize
        {
            get { return m_BoardSize; }
        }

        private bool m_IsAgainstComputer = true;
        public bool IsAgainstComputer
        {
            get { return m_IsAgainstComputer; }
        }

        public FormSettings()
        {
            InitializeComponent();
            updateBoardSizeLabel();
        }

        private void buttonBoardSize_Click(object sender, EventArgs e)
        {
            m_BoardSize += 2;
            if (m_BoardSize > 12)
            {
                m_BoardSize = 6;
            }
            updateBoardSizeLabel();
        }

        private void updateBoardSizeLabel()
        {
            buttonBoardSize.Text = $"Board Size: {m_BoardSize}x{m_BoardSize} (click to change)";
        }

        private void buttonOnePlayer_Click(object sender, EventArgs e)
        {
            m_IsAgainstComputer = true;
            startGame();
        }

        private void buttonTwoPlayers_Click(object sender, EventArgs e)
        {
            m_IsAgainstComputer = false;
            startGame();
        }

        private void startGame()
        {
            this.Hide();
            FormGame gameForm = new FormGame(BoardSize, IsAgainstComputer);
            gameForm.ShowDialog();
            this.Close();
        }
    }
}
