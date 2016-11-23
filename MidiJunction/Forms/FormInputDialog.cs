using System;
using System.Windows.Forms;

// ReSharper disable LocalizableElement

namespace MidiJunction.Forms
{
    public partial class FormInputDialog : Form
    {
        public FormInputDialog()
        {
            InitializeComponent();
        }

        public static string Execute(string title, string text, string defaultValue)
        {
            using (var form = new FormInputDialog())
            {
                form.Text = title;
                form.label1.Text = text;
                form.textBox1.Text = defaultValue;

                return form.ShowDialog() == DialogResult.OK ? form.textBox1.Text : null;
            }
        }
    }
}
