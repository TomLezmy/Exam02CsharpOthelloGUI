using System;
using System.Windows.Forms;

namespace Ex05_Othello
{
    internal class OptionForm : Form
    {
        private Button m_BoardSizeBtn;
        private Button m_VsComputerBtn;
        private Button m_TwoPlayerBtn;
        private int m_ChosenBoardSize;
        private bool m_IsVsComputer;

        public bool VsComputer
        {
            get
            {
                return m_IsVsComputer;
            }
        }
        
        public int BoardSize
        {
            get
            {
                return m_ChosenBoardSize;
            }
        }

        public OptionForm()
        {
            initializeForm();
            initializeComponents();
        }

        private void initializeForm()
        {
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = "Othello - Game Settings";
            this.ShowIcon = false;
            this.Width = 390;
            this.Height = 200;

            this.CenterToParent();
        }

        private void initializeComponents()
        {
            m_IsVsComputer = false;

            m_ChosenBoardSize = 6;
            m_BoardSizeBtn = new Button();
            m_BoardSizeBtn.Top = 15;
            m_BoardSizeBtn.Left = 15;
            m_BoardSizeBtn.Width = 345;
            m_BoardSizeBtn.Height = 50;
            m_BoardSizeBtn.Text = "Board Size : 6x6 (click to increase)";
            m_BoardSizeBtn.Click += M_BoardSizeBtn_Click;
            this.Controls.Add(m_BoardSizeBtn);

            m_VsComputerBtn = new Button();
            m_VsComputerBtn.Top = m_BoardSizeBtn.Bottom + 30;
            m_VsComputerBtn.Left = m_BoardSizeBtn.Left;
            m_VsComputerBtn.Width = 165;
            m_VsComputerBtn.Height = 50;
            m_VsComputerBtn.Text = "Play against the computer";
            m_VsComputerBtn.Click += VsBtn_Click;
            this.Controls.Add(m_VsComputerBtn);

            m_TwoPlayerBtn = new Button();
            m_TwoPlayerBtn.Top = m_BoardSizeBtn.Bottom + 30;
            m_TwoPlayerBtn.Width = 165;
            m_TwoPlayerBtn.Height = 50;
            m_TwoPlayerBtn.Left = m_BoardSizeBtn.Right - m_TwoPlayerBtn.Width;
            m_TwoPlayerBtn.Text = "Play against your friend";
            m_TwoPlayerBtn.Click += VsBtn_Click;
            this.Controls.Add(m_TwoPlayerBtn);
        }

        private void VsBtn_Click(object sender, EventArgs e)
        {
            if ((sender as Button).Equals(m_VsComputerBtn))
            {
                m_IsVsComputer = true;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void M_BoardSizeBtn_Click(object sender, EventArgs e)
        {
            if (m_ChosenBoardSize != 12)
            {
                m_ChosenBoardSize += 2;
            }
            else
            {
                m_ChosenBoardSize = 6;
            }

            m_BoardSizeBtn.Text = string.Format("Board Size : {0}x{0} (click to increase)", m_ChosenBoardSize);
        }

        private void M_Btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
