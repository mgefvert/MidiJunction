using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MidiJunction.Devices;

namespace MidiJunction.Forms
{
    public partial class FormMessageTrace : Form
    {
        private readonly Dictionary<int, List<MidiMessage>> _messages = new Dictionary<int, List<MidiMessage>>();
        private int _currentChannel;

        public FormMessageTrace()
        {
            InitializeComponent();
        }

        private void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (_messages.ContainsKey(_currentChannel))
            {
                var item = _messages[_currentChannel].ElementAtOrDefault(e.ItemIndex);
                if (item != null)
                {
                    e.Item = new ListViewItem(new[] { item.Time.ToString("HH:mm:ss.fff"), item.ToString() });
                    return;
                }
            }

            e.Item = new ListViewItem(new[] { "", "" });
        }

        public void AddMessage(MidiMessage msg)
        {
            if (!checkBox1.Checked)
                return;

            if (!_messages.ContainsKey(msg.Channel))
                _messages[msg.Channel] = new List<MidiMessage>();

            var channel = _messages[msg.Channel];
            channel.Insert(0, msg);
            if (channel.Count > 1000)
                channel.RemoveRange(1000, channel.Count - 1000);

            if (msg.Channel == _currentChannel)
                UpdateListView();
        }

        private void UpdateListView()
        {
            if (listView1.Visible)
                listView1.Invoke((Action)delegate
                {
                    listView1.VirtualListSize = _messages.ContainsKey(_currentChannel) ? _messages[_currentChannel].Count : 0;
                    listView1.Invalidate();
                });
        }

        public void Select(int channel)
        {
            _currentChannel = channel;
            UpdateListView();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Select(comboBox1.SelectedIndex);
        }

        private void FormTools_Shown(object sender, EventArgs e)
        {
            Select(_currentChannel);
        }

        private void FormTools_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void FormTools_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            Select(0);
        }
    }
}
