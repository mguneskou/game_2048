namespace Game2048
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_score = new System.Windows.Forms.Label();
            this.lbl_bestScore = new System.Windows.Forms.Label();
            this.pb = new System.Windows.Forms.PictureBox();
            this.lbl_gameOver = new System.Windows.Forms.Label();
            this.btn_newGame = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pb)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_score
            // 
            this.lbl_score.AutoSize = true;
            this.lbl_score.BackColor = System.Drawing.Color.Black;
            this.lbl_score.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_score.ForeColor = System.Drawing.Color.LimeGreen;
            this.lbl_score.Location = new System.Drawing.Point(214, 9);
            this.lbl_score.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_score.Name = "lbl_score";
            this.lbl_score.Size = new System.Drawing.Size(55, 20);
            this.lbl_score.TabIndex = 1;
            this.lbl_score.Text = "Score:";
            this.lbl_score.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_bestScore
            // 
            this.lbl_bestScore.AutoSize = true;
            this.lbl_bestScore.BackColor = System.Drawing.Color.Black;
            this.lbl_bestScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_bestScore.ForeColor = System.Drawing.Color.LimeGreen;
            this.lbl_bestScore.Location = new System.Drawing.Point(214, 51);
            this.lbl_bestScore.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_bestScore.Name = "lbl_bestScore";
            this.lbl_bestScore.Size = new System.Drawing.Size(46, 20);
            this.lbl_bestScore.TabIndex = 3;
            this.lbl_bestScore.Text = "Best:";
            this.lbl_bestScore.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pb
            // 
            this.pb.BackColor = System.Drawing.Color.White;
            this.pb.Location = new System.Drawing.Point(9, 93);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(400, 400);
            this.pb.TabIndex = 4;
            this.pb.TabStop = false;
            // 
            // lbl_gameOver
            // 
            this.lbl_gameOver.AutoSize = true;
            this.lbl_gameOver.BackColor = System.Drawing.Color.Black;
            this.lbl_gameOver.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_gameOver.ForeColor = System.Drawing.Color.Red;
            this.lbl_gameOver.Location = new System.Drawing.Point(103, 275);
            this.lbl_gameOver.Name = "lbl_gameOver";
            this.lbl_gameOver.Size = new System.Drawing.Size(210, 37);
            this.lbl_gameOver.TabIndex = 5;
            this.lbl_gameOver.Text = "GAME OVER";
            this.lbl_gameOver.Visible = false;
            // 
            // btn_newGame
            // 
            this.btn_newGame.AutoSize = true;
            this.btn_newGame.BackColor = System.Drawing.Color.Black;
            this.btn_newGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_newGame.ForeColor = System.Drawing.Color.Lime;
            this.btn_newGame.Location = new System.Drawing.Point(9, 23);
            this.btn_newGame.Name = "btn_newGame";
            this.btn_newGame.Size = new System.Drawing.Size(196, 37);
            this.btn_newGame.TabIndex = 6;
            this.btn_newGame.Text = "NEW GAME";
            this.btn_newGame.Click += new System.EventHandler(this.btn_newGame_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 500);
            this.Controls.Add(this.btn_newGame);
            this.Controls.Add(this.lbl_gameOver);
            this.Controls.Add(this.pb);
            this.Controls.Add(this.lbl_bestScore);
            this.Controls.Add(this.lbl_score);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "2048 Game";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pb)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbl_score;
        private System.Windows.Forms.Label lbl_bestScore;
        private System.Windows.Forms.PictureBox pb;
        private System.Windows.Forms.Label lbl_gameOver;
        private System.Windows.Forms.Label btn_newGame;
    }
}

