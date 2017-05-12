    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
