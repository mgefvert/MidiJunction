using System;
using System.Windows.Forms;
using MidiJunction.Forms;

namespace MidiJunction
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.Run(new FormMain());
        }
    }
}
