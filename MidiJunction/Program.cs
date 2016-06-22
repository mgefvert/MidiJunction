using System;
using System.Windows.Forms;

namespace MidiJunction
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
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
