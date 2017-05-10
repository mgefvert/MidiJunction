using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DotNetCommons;
using MidiJunction.Classes;
using MidiJunction.Configuration;
using MidiJunction.Devices;

// ReSharper disable LocalizableElement

namespace MidiJunction.Forms
{
    public partial class FormSettings : Form
    {
        private readonly Config _config;
        private List<string> _devices;
        private List<ConfigButton> _buttons;
        private bool _restartRequired;

        public FormSettings(Config config)
        {
            InitializeComponent();
            _config = config;
        }

        private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void AddBus_Click(object sender, EventArgs e)
        {
            if (_devices.Count >= 4)
            {
                MessageBox.Show("Maximum number of output devices exceeded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var form = new FormSettingDevice())
            {
                if (!form.Execute())
                    return;

                var deviceName = form.DeviceName;
                if (!ValidateOutputDevice(deviceName))
                    return;

                _devices.Add(deviceName);
                _restartRequired = true;
                listView1.VirtualListSize = _devices.Count;
            }
        }

        private void AddInstrument_Click(object sender, EventArgs e)
        {
            var button = new ConfigButton();

            using (var form = new FormSettingButton { Button = button })
            {
                if (!form.Execute())
                    return;

                if (!ValidateInstrument(form.Button))
                    return;

                _buttons.Add(form.Button);
                listView2.VirtualListSize = _buttons.Count;
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            _config.Buttons.Clear();
            _config.Buttons.AddRange(_buttons);
            _config.DefaultChannel = Helper.Limit((int)midiChannel.Value - 1, 0, 15);
            _config.InputDevice = inputDevice.Text.Trim();
            _config.ControlOnAllChannels = checkBox1.Checked;
            _config.LowKeysControlHotKeys = checkBox2.Checked;
            _config.OutputDevices.Clear();
            _config.OutputDevices.AddRange(_devices);
            _config.Save();

            if (_restartRequired)
                MessageBox.Show("Changing the virtual MIDI outputs requires a program restart.", "Virtual MIDI changed",
                  MessageBoxButtons.OK, MessageBoxIcon.Information);

            _config.TriggerUpdated();
            Hide();
        }

        private void EditBus_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
                return;

            var index = listView1.SelectedIndices.Cast<int>().First();
            using (var form = new FormSettingDevice { DeviceName = _devices[index] })
            {
                if (!form.Execute())
                    return;

                if (!ValidateOutputDevice(form.DeviceName))
                    return;

                _devices[index] = form.DeviceName;
                _restartRequired = true;
                listView1.Invalidate();
            }
        }

        private void EditInstrument_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedIndices.Count == 0)
                return;

            var index = listView2.SelectedIndices.Cast<int>().First();
            var button = _buttons[index].Clone();

            using (var form = new FormSettingButton { Button = button })
            {
                if (!form.Execute())
                    return;

                if (!ValidateInstrument(form.Button))
                    return;

                _buttons[index] = form.Button;
                listView2.Invalidate();
            }
        }

        private void ListViewBus_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            e.Item = new ListViewItem(new[]
            {
                ((char)(65 + e.ItemIndex)).ToString(),
                _devices.ElementAtOrDefault(e.ItemIndex)
            });
        }

        private void ListViewInstrument_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            var button = _buttons[e.ItemIndex];

            e.Item = new ListViewItem(new[]
            {
                Helper.KeyToString(button.Key),
                ((char)(65 + button.Device)).ToString(),
                (button.Channel + 1).ToString(),
                button.Name,
                Helper.EnumToString(button.BreakAfter, true)
            });
        }

        private void MoveBusDown(object sender, EventArgs e)
        {
            MoveUpDown(listView1, _devices, 1);
        }

        private void MoveBusUp(object sender, EventArgs e)
        {
            MoveUpDown(listView1, _devices, -1);
        }

        private void MoveInstrumentDown(object sender, EventArgs e)
        {
            MoveUpDown(listView2, _buttons, 1);
        }

        private void MoveInstrumentUp(object sender, EventArgs e)
        {
            MoveUpDown(listView2, _buttons, -1);
        }

        private static void MoveUpDown<T>(ListView listview, List<T> items, int offset)
        {
            if (listview.SelectedIndices.Count == 0)
                return;

            var current = listview.SelectedIndices.Cast<int>().FirstOrDefault();
            if (items.Swap(current, current + offset))
            {
                listview.SelectedIndices.Clear();
                listview.SelectedIndices.Add(current + offset);
                listview.Invalidate();
            }
        }

        private void RemoveBus_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
                return;

            var index = listView1.SelectedIndices.Cast<int>().First();
            _devices.RemoveAt(index);
            listView1.VirtualListSize = _devices.Count;
        }

        private void RemoveInstrument_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedIndices.Count == 0)
                return;

            var index = listView2.SelectedIndices.Cast<int>().First();
            _buttons.RemoveAt(index);
            listView2.VirtualListSize = _buttons.Count;
        }

        private bool ValidateInstrument(ConfigButton button)
        {
            if (string.IsNullOrEmpty(button.Name))
                return false;

            if (button.Device < 0 || button.Device >= 4 || button.Channel < 0 || button.Channel >= 16)
            {
                MessageBox.Show("Device or channel outside of allowed range.", "Error",
                  MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        private bool ValidateOutputDevice(string deviceName)
        {
            if (string.IsNullOrEmpty(deviceName))
                return false;

            if (_devices.Contains(deviceName, StringComparer.CurrentCultureIgnoreCase))
            {
                MessageBox.Show("Device name already exists.", "Error",
                  MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        private void FormSettings_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible)
                return;

            InitializeData();
        }

        private void InitializeData()
        {
            _devices = _config.OutputDevices.ToList();
            _buttons = _config.Buttons.Select(x => x.Clone()).ToList();

            midiChannel.Value = Helper.Limit(_config.DefaultChannel + 1, 1, 16);
            inputDevice.Text = _config.InputDevice;
            checkBox1.Checked = _config.ControlOnAllChannels;
            checkBox2.Checked = _config.LowKeysControlHotKeys;

            inputDevice.Items.Clear();
            for (int i = 0; i < MidiDeviceManager.InDeviceCount; i++)
            {
                var info = MidiDeviceManager.InDeviceInfo(i);
                var name = (info.szPname ?? "").Trim();
                if (!string.IsNullOrEmpty(name) && !_config.OutputDevices.Contains(name, StringComparer.CurrentCultureIgnoreCase))
                    inputDevice.Items.Add(name);
            }

            listView1.VirtualListSize = _devices.Count;
            listView2.VirtualListSize = _buttons.Count;
        }
    }
}
