using System;
using System.Windows.Forms;
using MidiJunction.Forms;

namespace MidiJunction
{
    static class Program
    {
        public static FormMain MainForm;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            MainForm = new FormMain();
            Application.Run(MainForm);
        }
    }
}
