using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MidiJunction.Forms
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.tobias-erichsen.de/software/virtualmidi.html");
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/mgefvert/MidiJunction");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
