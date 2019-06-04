using System;
using System.Windows.Forms;

namespace Ex05_Othello
{
    class OptionForm : Form
    {
        private TextBox m_FirstUserName;
        private Label m_FirstUserLabel;
        private TextBox m_SecondUserName;
        private Label m_SecondUserLabel;
        private Button m_StartBtn;
        private CheckBox m_VsComputer;
        private RadioButton m_SizeSix;
        private RadioButton m_SizeEight;
        private Label m_SizeLabel;

        public string FirstUserName
        {
            get
            {
                return m_FirstUserName.Text;
            }
        }

        public string SecondUserName
        {
            get
            {
                return m_SecondUserName.Text;
            }
        }

        public bool VsComputer
        {
            get
            {
                return m_VsComputer.Checked;
            }
        }

        public int BoardSize
        {
            get
            {
                int size = int.Parse(m_SizeEight.Text);

                if (m_SizeSix.Checked)
                {
                    size = int.Parse(m_SizeSix.Text);
                }

                return size;
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
            this.Text = "Options";
            this.Width = 250;
            this.Height = 230;

            this.CenterToParent();
        }

        private void initializeComponents()
        {
            m_FirstUserLabel = new Label();
            m_FirstUserLabel.Top = 20;
            m_FirstUserLabel.Left = 10;
            m_FirstUserLabel.Text = "First User Name:";
            m_FirstUserLabel.AutoSize = true;
            this.Controls.Add(m_FirstUserLabel);


            m_SecondUserLabel = new Label();
            m_SecondUserLabel.Top = m_FirstUserLabel.Top + 30;
            m_SecondUserLabel.Left = m_FirstUserLabel.Left;
            m_SecondUserLabel.Text = "Second User Name:";
            m_SecondUserLabel.AutoSize = true;
            this.Controls.Add(m_SecondUserLabel);

            m_SecondUserName = new TextBox();
            m_SecondUserName.Top = m_SecondUserLabel.Top;// - m_SecondUserLabel.Height / 2;
            m_SecondUserName.Left = m_SecondUserLabel.Right + 5;
            this.Controls.Add(m_SecondUserName);

            m_FirstUserName = new TextBox();
            m_FirstUserName.Top = m_FirstUserLabel.Top;// - (m_FirstUserLabel.Height / 2 + m_FirstUserName.Height);
            m_FirstUserName.Left = m_SecondUserLabel.Right + 5;
            this.Controls.Add(m_FirstUserName);

            m_VsComputer = new CheckBox();
            m_VsComputer.Text = "VS Computer";
            m_VsComputer.Top = m_SecondUserLabel.Top + 30;
            m_VsComputer.Left = m_FirstUserLabel.Left;
            m_VsComputer.CheckedChanged += M_VsComputer_CheckedChanged;
            this.Controls.Add(m_VsComputer);

            m_SizeLabel = new Label();
            m_SizeLabel.Top = m_VsComputer.Top + 30;
            m_SizeLabel.Left = m_FirstUserLabel.Left;
            m_SizeLabel.Text = "Board Size:";
            m_SizeLabel.AutoSize = true;
            this.Controls.Add(m_SizeLabel);


            m_SizeSix = new RadioButton();
            m_SizeSix.Top = m_SizeLabel.Top;
            m_SizeSix.Left = m_SizeLabel.Right + 5;
            m_SizeSix.Text = "6";
            m_SizeSix.AutoSize = true;
            this.Controls.Add(m_SizeSix);

            m_SizeEight = new RadioButton();
            m_SizeEight.Top = m_SizeLabel.Top;
            m_SizeEight.Left = m_SizeSix.Right + 5;
            m_SizeEight.Text = "8";
            m_SizeEight.AutoSize = true;
            m_SizeEight.Checked = true;
            this.Controls.Add(m_SizeEight);

            m_StartBtn = new Button();
            m_StartBtn.Top = m_SizeLabel.Top + 40;
            m_StartBtn.Left = m_FirstUserLabel.Left;
            m_StartBtn.Text = "Save";
            m_StartBtn.Click += M_Btn_Click;
            this.Controls.Add(m_StartBtn);
        }

        private void M_Btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void M_VsComputer_CheckedChanged(object sender, EventArgs e)
        {
            if (m_VsComputer.Checked)
            {
                m_SecondUserName.Enabled = false;
            }
            else
            {
                m_SecondUserName.Enabled = true;
            }
        }
    }
}
