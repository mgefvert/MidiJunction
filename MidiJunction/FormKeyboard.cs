using System;
using System.Windows.Forms;

namespace MidiJunction
{
  public partial class FormKeyboard : Form
  {
    public FormKeyboard()
    {
      InitializeComponent();
    }

    private void FormKeyboard_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing)
      {
        e.Cancel = true;
        Hide();
      }
    }
  }
}
