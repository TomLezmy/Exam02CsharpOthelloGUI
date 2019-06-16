using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace Ex05_Othello
{
    internal class GameForm : Form
    {
        private const int k_BtnSize = 40;
        private const int k_BtnStartPosition = 20;
        private Game m_GameModel;
        private Image[] m_Images;
        private string[] m_PieceNames;
        private PictureBox[,] m_BoardPieces;

        public GameForm(int i_BoardSize, bool i_VsComputer)
        {
            m_GameModel = new Game();
            m_Images = new Image[2] { B19_Ex05_Matan_205838618_Tom_206325409.Properties.Resources.CoinWhite, B19_Ex05_Matan_205838618_Tom_206325409.Properties.Resources.CoinBlack };
            m_PieceNames = new string[2] { "White's", "Black's" };
            m_GameModel.BoardSize = i_BoardSize;
            m_GameModel.InitBoard();
            initializeComponents(i_BoardSize);
            m_GameModel.IsTwoPlayer = !i_VsComputer;
        }
        
        private void initializeComponents(int i_Size)
        {
            initializeForm();
            initializeBoard(i_Size);
            updateBoard();
        }

        private void initializeForm()
        {
            this.Width = (m_GameModel.BoardSize * k_BtnSize) + (k_BtnStartPosition * 3);
            this.Height = (m_GameModel.BoardSize * k_BtnSize) + (k_BtnStartPosition * 4);
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = "Othello";
        }

        private void initializeBoard(int i_Size)
        {
            m_BoardPieces = new PictureBox[m_GameModel.BoardSize, m_GameModel.BoardSize];
            for (int i = 0; i < i_Size; i++)
            {
                for (int j = 0; j < i_Size; j++)
                {
                    m_BoardPieces[i, j] = new PictureBox();
                    m_BoardPieces[i, j].Size = new Size(k_BtnSize, k_BtnSize);
                    m_BoardPieces[i, j].Location = new Point((i * k_BtnSize) + k_BtnStartPosition, (j * k_BtnSize) + k_BtnStartPosition);
                    m_BoardPieces[i, j].BorderStyle = BorderStyle.Fixed3D;
                    m_BoardPieces[i, j].Name = string.Format("Btn{0},{1}", (char)(i + 'A'), j + 1);
                    m_BoardPieces[i, j].Click += M_BoardPieces_Click;
                    m_BoardPieces[i, j].TabStop = false;
                    this.Controls.Add(m_BoardPieces[i, j]);
                }
            }
        }

        private void updateBoard()
        {
            List<string> possibleMoves = m_GameModel.PossibleMoves;
            string currentPiece;

            for (int i = 0; i < m_GameModel.BoardSize; i++)
            {
                for (int j = 0; j < m_GameModel.BoardSize; j++)
                {
                    currentPiece = (char)(i + 'A') + (j + 1).ToString();
                    if(m_GameModel.Board[j, i] == 0)
                    {
                        m_BoardPieces[i, j].BackgroundImage = null;
                    }

                    if (possibleMoves.Contains(currentPiece))
                    {
                        m_BoardPieces[i, j].BackColor = Color.LimeGreen;
                        m_BoardPieces[i, j].Enabled = true;
                    }
                    else
                    {
                        m_BoardPieces[i, j].BackColor = Color.Transparent;
                        m_BoardPieces[i, j].Enabled = false;
                        if (m_GameModel.Board[j, i] != 0)
                        {
                            m_BoardPieces[i, j].BackgroundImage = m_Images[m_GameModel.Board[j, i] - 1];
                            m_BoardPieces[i, j].BackgroundImageLayout = ImageLayout.Stretch;
                        }
                    }
                }
            }

            this.Text = string.Format("Othello - {0} Turn", m_PieceNames[m_GameModel.PlayerTurn]);
        }

        private void M_BoardPieces_Click(object sender, EventArgs e)
        {
            string[] split = (sender as PictureBox).Name.Split(',');
            string pick = split[0][3] + split[1];

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
                        while (!m_GameModel.CheckAvailableMoves() && !m_GameModel.GameOver)
                        {
                            skipTurn();
                            if (m_GameModel.CheckAvailableMoves())
                            {
                                computerTurn();
                            }
                        }
                    }
                }
            }

            if (m_GameModel.GameOver)
            {
                gameOver();
            }
        }

        private void skipTurn()
        {
            m_GameModel.HandleNoAvailableMoves();
            m_GameModel.NextTurn();
            if (!m_GameModel.GameOver)
            {
                MessageBox.Show("No moves available!\nSkipping turn", "Othello", MessageBoxButtons.OK, MessageBoxIcon.Information);
                updateBoard();
            }
        }

        private void gameOver()
        {
            string gameResultStr;
            m_GameModel.FinishGame();

            if (m_GameModel.IsFirstPlayerWon())
            {
                gameResultStr = string.Format(
                    "White Won!! ({0}/{1})({2}/{3})",
                    m_GameModel.FirstUserScore,
                    m_GameModel.SecondUserScore,
                    m_GameModel.FirstUserWins,
                    m_GameModel.SecondUserWins);
            }
            else
            {
                if (m_GameModel.IsSecondPlayerWon())
                {
                    gameResultStr = string.Format(
                    "Black Won!! ({0}/{1})({2}/{3})",
                    m_GameModel.SecondUserScore,
                    m_GameModel.FirstUserScore,
                    m_GameModel.SecondUserWins,
                    m_GameModel.FirstUserWins);
                }
                else
                {
                    gameResultStr = "Game ended in tie!!";
                }
            }

            DialogResult dialogResult = MessageBox.Show(string.Format("{0}\nWould you like another round?", gameResultStr), "Othello", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
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
