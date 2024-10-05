namespace OthelloWinForms
{
    partial class FormSettings
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button buttonBoardSize;
        private System.Windows.Forms.Button buttonOnePlayer;
        private System.Windows.Forms.Button buttonTwoPlayers;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.buttonBoardSize = new System.Windows.Forms.Button();
            this.buttonOnePlayer = new System.Windows.Forms.Button();
            this.buttonTwoPlayers = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonBoardSize
            // 
            this.buttonBoardSize.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonBoardSize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonBoardSize.ForeColor = System.Drawing.Color.White;
            this.buttonBoardSize.Location = new System.Drawing.Point(22, 19);
            this.buttonBoardSize.Name = "buttonBoardSize";
            this.buttonBoardSize.Size = new System.Drawing.Size(406, 47);
            this.buttonBoardSize.TabIndex = 0;
            this.buttonBoardSize.Text = "Board Size: 6x6 (click to change)";
            this.buttonBoardSize.UseVisualStyleBackColor = false;
            this.buttonBoardSize.Click += new System.EventHandler(this.buttonBoardSize_Click);
            // 
            // buttonOnePlayer
            // 
            this.buttonOnePlayer.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonOnePlayer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonOnePlayer.ForeColor = System.Drawing.Color.White;
            this.buttonOnePlayer.Location = new System.Drawing.Point(22, 85);
            this.buttonOnePlayer.Name = "buttonOnePlayer";
            this.buttonOnePlayer.Size = new System.Drawing.Size(189, 57);
            this.buttonOnePlayer.TabIndex = 1;
            this.buttonOnePlayer.Text = "Play against the computer";
            this.buttonOnePlayer.UseVisualStyleBackColor = false;
            this.buttonOnePlayer.Click += new System.EventHandler(this.buttonOnePlayer_Click);
            // 
            // buttonTwoPlayers
            // 
            this.buttonTwoPlayers.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonTwoPlayers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonTwoPlayers.ForeColor = System.Drawing.Color.White;
            this.buttonTwoPlayers.Location = new System.Drawing.Point(227, 85);
            this.buttonTwoPlayers.Name = "buttonTwoPlayers";
            this.buttonTwoPlayers.Size = new System.Drawing.Size(201, 57);
            this.buttonTwoPlayers.TabIndex = 2;
            this.buttonTwoPlayers.Text = "Play against a friend";
            this.buttonTwoPlayers.UseVisualStyleBackColor = false;
            this.buttonTwoPlayers.Click += new System.EventHandler(this.buttonTwoPlayers_Click);
            // 
            // FormSettings
            // 
            this.ClientSize = new System.Drawing.Size(453, 162);
            this.Controls.Add(this.buttonTwoPlayers);
            this.Controls.Add(this.buttonOnePlayer);
            this.Controls.Add(this.buttonBoardSize);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Othello - Game Settings";
            this.ResumeLayout(false);

        }
    }
}
