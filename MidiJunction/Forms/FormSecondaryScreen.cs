using System;
using System.Windows.Forms;

namespace MidiJunction.Forms
{
    public partial class FormSecondaryScreen : Form
    {
        public FormSecondaryScreen()
        {
            InitializeComponent();
        }

        private void FormSecondaryScreen_Load(object sender, EventArgs e)
        {
            labelDevices.Text = "";
            labelChord.Text = "";
        }
    }
}
