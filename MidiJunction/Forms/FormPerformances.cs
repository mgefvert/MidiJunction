using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MidiJunction.Configuration;

// ReSharper disable LocalizableElement

namespace MidiJunction.Forms
{
    public partial class FormPerformances : Form
    {
        private readonly Config _config;
        private readonly ConfigPerformances _performances;
        private List<ConfigPerformance> _selection;

        public FormPerformances(Config config, ConfigPerformances performances)
        {
            InitializeComponent();
            _config = config;
            _performances = performances;
        }

        private void Filter(string filter)
        {
            var selection = (IEnumerable<ConfigPerformance>)_performances.Values;
            if (!string.IsNullOrEmpty(filter))
                selection = selection.Where(x => x.Title.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) != -1);

            _selection = selection.OrderBy(x => x.Title).ToList();

            listView1.VirtualListSize = _selection.Count;
        }

        private void FormPerformances_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void FormPerformances_Load(object sender, EventArgs e)
        {
            Filter(null);
            ActiveControl = textBox1;
        }

        private void FormPerformances_Resize(object sender, EventArgs e)
        {
            columnHeader1.Width = listView1.ClientSize.Width - columnHeader2.Width - 20;
        }

        private void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            var item = _selection.ElementAtOrDefault(e.ItemIndex);
            e.Item = item == null 
                ? new ListViewItem(new[] { "", "N/A" }) 
                : new ListViewItem(new[] { item.FKey != null ? "F" + item.FKey : "", item.Title ?? "" });
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Filter(textBox1.Text);
        }

        private void FormPerformances_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                textBox1.Text = "";
                Filter(null);
                ActiveControl = textBox1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var item = GetSelectedItem();
            if (item == null)
                return;

            var result = FormInputDialog.Execute("Performance title", item.Title, null);
            if (string.IsNullOrEmpty(result))
                return;

            item.Title = result;
            _config.Save();

            listView1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var item = GetSelectedItem();
            if (item == null)
                return;

            if (MessageBox.Show($"Are you sure you want to delete the performance '{item.Title}'?",
                    "Delete performance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            _performances.RemovePerformance(item.Title);
            _config.Save();

            Filter(textBox1.Text);
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            var item = GetSelectedItem();
            if (item == null)
                return;

            if (e.KeyCode == Keys.F12)
            {
                MessageBox.Show("F12 can not be assigned.", "Key assignment", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (e.KeyCode >= Keys.F1 && e.KeyCode <= Keys.F11)
            {
                var key = e.KeyCode - Keys.F1 + 1;

                if (MessageBox.Show($"Bind F{key} to performance '{item.Title}'?",
                        "Key assignment", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                _performances.ClearPerformanceKey(e.KeyCode);
                item.FKey = key;
                _config.Save();

                listView1.Refresh();
            }
        }

        private ConfigPerformance GetSelectedItem()
        {
            return listView1.SelectedIndices.Count != 1 ? null : _selection.ElementAtOrDefault(listView1.SelectedIndices[0]);
        }

        private void buttonClearFKey_Click(object sender, EventArgs e)
        {
            var item = GetSelectedItem();
            if (item == null)
                return;

            item.FKey = null;
            _config.Save();
            listView1.Refresh();
        }

        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clear all function key bindings?", "Key assignment", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            foreach (var item in _performances)
                item.Value.FKey = null;

            _config.Save();
            listView1.Refresh();
        }
    }
}
