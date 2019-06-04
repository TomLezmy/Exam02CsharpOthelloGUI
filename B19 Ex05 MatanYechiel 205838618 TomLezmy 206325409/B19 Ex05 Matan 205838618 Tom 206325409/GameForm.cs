using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace Ex05_Othello
{
    class GameForm : Form
    {
        private OptionForm m_OptionForm;
        private Game m_GameModel;
        private Image[] m_Images;
        private MenuStrip m_MenuStrip;
        private ToolStripMenuItem m_FileToolStripMenuItem;
        private ToolStripMenuItem m_NewGameToolStripMenuItem;
        private ToolStripMenuItem m_HintToolStripMenuItem;
        private StatusStrip m_StatusStrip;
        private ToolStripStatusLabel m_StatusStripLabel;
        private Button[,] m_ButtonBoard;
        private const int k_BtnSize = 40;
        private const int k_BtnStartPosition = 30;

        public GameForm()
        {
            m_OptionForm = new OptionForm();
            m_OptionForm.ShowDialog();
            m_GameModel = new Game();
            m_Images = new Image[] { Image.FromFile(@".\WhitePiece.png"), Image.FromFile(@".\BlackPiece.png") };
            m_GameModel.BoardSize = m_OptionForm.BoardSize;
            m_GameModel.InitBoard();
            initializeComponents(m_OptionForm.BoardSize);
            if (m_OptionForm.VsComputer)
            {
                m_GameModel.SetIsTwoPlayer("c");
            }
            else
            {
                m_GameModel.SetIsTwoPlayer("p");
            }
        }

        private void newGame()
        {
            m_GameModel.BoardSize = m_OptionForm.BoardSize;
            m_GameModel.InitBoard();
            initializeBoard(m_OptionForm.BoardSize);
            updateBoard();
            if (m_OptionForm.VsComputer)
            {
                m_GameModel.SetIsTwoPlayer("c");
            }
            else
            {
                m_GameModel.SetIsTwoPlayer("p");
            }
        }

        private void initializeComponents(int i_Size)
        {
            initializeForm();
            initializeBoard(i_Size);
            updateBoard();
        }

        private void initializeForm()
        {
            this.Width = 450;
            this.Height = 450;
            this.CenterToScreen();
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = "Othello";
            this.BackColor = Color.DarkGreen;

            m_MenuStrip = new MenuStrip();
            m_FileToolStripMenuItem = new ToolStripMenuItem();
            m_NewGameToolStripMenuItem = new ToolStripMenuItem();
            m_HintToolStripMenuItem = new ToolStripMenuItem();
            m_FileToolStripMenuItem.Text = "File";
            m_NewGameToolStripMenuItem.Text = "New Game";
            m_NewGameToolStripMenuItem.Click += NewGameToolStripMenuItem_Click;
            m_HintToolStripMenuItem.Text = "Hint";
            m_HintToolStripMenuItem.Click += M_HintToolStripMenuItem_Click;

            m_FileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { m_NewGameToolStripMenuItem, m_HintToolStripMenuItem });
            m_MenuStrip.Items.Add(m_FileToolStripMenuItem);
            this.Controls.Add(m_MenuStrip);

            m_StatusStrip = new StatusStrip();
            m_StatusStripLabel = new ToolStripStatusLabel();
            m_StatusStripLabel.BackColor = Color.Transparent;
            m_StatusStrip.Items.Add(m_StatusStripLabel);
            this.Controls.Add(m_StatusStrip);
        }

        private void M_HintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string hint = "Available Moves:" + Environment.NewLine;

            foreach (var item in m_GameModel.PossibleMoves)
            {
                hint += item + Environment.NewLine;
            }

            MessageBox.Show(hint);
        }

        private void initializeBoard(int i_Size)
        {
            m_ButtonBoard = new Button[m_GameModel.BoardSize, m_GameModel.BoardSize];
            for (int i = 0; i < i_Size; i++)
            {
                for (int j = 0; j < i_Size; j++)
                {
                    m_ButtonBoard[i, j] = new Button();
                    m_ButtonBoard[i, j].Size = new Size(k_BtnSize, k_BtnSize);
                    m_ButtonBoard[i, j].Location = new Point((i * k_BtnSize), (j * k_BtnSize) + k_BtnStartPosition);
                    m_ButtonBoard[i, j].FlatStyle = FlatStyle.Flat;
                    m_ButtonBoard[i, j].Name = string.Format("Btn{0},{1}", ((char)(i + 'A')), j + 1);
                    m_ButtonBoard[i, j].Click += Btn_Click;
                    m_ButtonBoard[i, j].TabStop = false;
                    this.Controls.Add(m_ButtonBoard[i, j]);
                }
            }

        }

        private void NewGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_GameModel.InitBoard();
            updateBoard();
        }

        private void updateBoard()
        {
            for (int i = 0; i < m_GameModel.BoardSize; i++)
            {
                for (int j = 0; j < m_GameModel.BoardSize; j++)
                {
                    if (m_GameModel.Board[j, i] != 0)
                    {
                        m_ButtonBoard[i, j].BackgroundImage = m_Images[m_GameModel.Board[j, i] - 1];
                        m_ButtonBoard[i, j].BackgroundImageLayout = ImageLayout.Stretch;
                        m_ButtonBoard[i, j].Enabled = false;
                    }
                    else
                    {
                        m_ButtonBoard[i, j].BackgroundImage = null;
                        m_ButtonBoard[i, j].Enabled = true;
                    }
                }
            }

            m_StatusStripLabel.Text = string.Format("Player {0} turn     P1:{1} P2:{2}", (m_GameModel.PlayerTurn + 1).ToString(), m_GameModel.FirstUserScore, m_GameModel.SecondUserScore);
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            string[] split = (sender as Button).Name.Split(',');
            string pick = split[0][3] + split[1];

            if (!m_GameModel.PossibleMoves.Contains(pick))
            {
                MessageBox.Show("Invalid Move!");
            }
            else
            {
                m_GameModel.PlayTurn(int.Parse(split[1]) - 1, (split[0][3] - 'A' + 1) - 1);
                updateBoard();
                this.Refresh();

                if (!m_GameModel.CheckAvailableMoves())
                {
                    skipTurn();
                }
                else
                {
                    if (!m_GameModel.IsTwoPlayer)
                    {
                        computerTurn();
                        if (!m_GameModel.CheckAvailableMoves())
                        {
                            skipTurn();
                        }
                    }
                }
                if (m_GameModel.GameOver)
                {
                    gameOver();
                }
            }
        }

        private void skipTurn()
        {
            m_GameModel.HandleNoAvailableMoves();
            if (!m_GameModel.GameOver)
            {
                updateBoard();
            }
        }

        private void gameOver()
        {
            string winningPlayer;

            if (m_GameModel.IsFirstPlayerWon())
            {
                winningPlayer = m_OptionForm.FirstUserName;
            }
            else
            {
                if (m_GameModel.IsTwoPlayer)
                {
                    winningPlayer = m_OptionForm.SecondUserName;
                }
                else
                {
                    winningPlayer = "Computer";
                }
            }

            DialogResult dialogResult = MessageBox.Show(string.Format("{0} Won!\nPlayAgain?", winningPlayer), "Game Over", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                m_GameModel.InitBoard();
                updateBoard();
            }
            else
            {
                this.Close();
            }
        }

        private void computerTurn()
        {
            m_GameModel.GetTurn();
            updateBoard();
        }
    }
}
