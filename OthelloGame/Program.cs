using System;
using System.Windows.Forms;

namespace OthelloWinForms
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormSettings settingsForm = new FormSettings();
            Application.Run(settingsForm);
        }
    }
}
