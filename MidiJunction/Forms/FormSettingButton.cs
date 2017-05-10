using System;
using System.Linq;
using System.Windows.Forms;
using MidiJunction.Classes;
using MidiJunction.Configuration;

namespace MidiJunction.Forms
{
    public partial class FormSettingButton : Form
    {
        public ConfigButton Button { get; set; }

        public BreakType BreakType
        {
            get
            {
                return radioBreakNewLine.Checked
                    ? BreakType.NewLine
                    : (radioBreakSeparator.Checked ? BreakType.Separator : BreakType.None);
            }
            set
            {
                switch (value)
                {
                    case BreakType.Separator:
                        radioBreakSeparator.Checked = true;
                        break;
                    case BreakType.NewLine:
                        radioBreakNewLine.Checked = true;
                        break;
                    default:
                        radioNoBreak.Checked = true;
                        break;
                }
            }
        }

        public FormSettingButton()
        {
            InitializeComponent();

            comboBox3.Items.Clear();
            comboBox3.Items.AddRange(Config.AllowedKeys.Select(Helper.KeyToString).Cast<object>().ToArray());
        }

        public bool Execute()
        {
            ActiveControl = textBox1;
            textBox1.Text = Button.Name;
            comboBox1.SelectedIndex = Button.Device;
            comboBox2.SelectedIndex = Button.Channel;
            comboBox3.Text = Helper.KeyToString(Button.Key);
            BreakType = Button.BreakAfter;

            if (ShowDialog() != DialogResult.OK)
                return false;

            Button.Name = textBox1.Text.Trim();
            Button.Device = comboBox1.SelectedIndex;
            Button.Channel = comboBox2.SelectedIndex;
            Button.Key = Helper.StringToKey(comboBox3.Text);
            Button.BreakAfter = BreakType;
            return true;
        }
    }
}
