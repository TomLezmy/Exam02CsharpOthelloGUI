using System;

namespace Ex05_Othello
{
    internal class Program
    {
        public static void Main()
        {
            OptionForm optionForm = new OptionForm();
            optionForm.ShowDialog();
            if (optionForm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                GameForm gameForm = new GameForm(optionForm.BoardSize, optionForm.VsComputer);
                gameForm.ShowDialog();
            }
        }
    }
}
