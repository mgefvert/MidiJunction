using System;
using System.Windows.Forms;

namespace MidiJunction.Forms
{
    public partial class FormSettingDevice : Form
    {
        public string DeviceName
        {
            get => textBox1.Text.Trim();
            set => textBox1.Text = value;
        }

        public FormSettingDevice()
        {
            InitializeComponent();
        }

        public bool Execute()
        {
            ActiveControl = textBox1;
            return ShowDialog() == DialogResult.OK;
        }
    }
}
