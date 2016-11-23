using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonNetTools;
using MidiJunction.Configuration;
using MidiJunction.Controls;
using MidiJunction.Devices;

// ReSharper disable LocalizableElement

namespace MidiJunction.Forms
{
    public partial class FormMain : AppBarForm
    {
        public class KeyMapEntry
        {
            public Keys StartKey { get; }
            public Keys EndKey { get; }
            public bool Shift { get; }
            public Action<Keys> Action { get; }

            public KeyMapEntry(Keys startKey, Keys endKey, bool shift, Action<Keys> action)
            {
                StartKey = startKey;
                EndKey = endKey;
                Shift = shift;
                Action = action;
            }

            public KeyMapEntry(Keys key, bool shift, Action<Keys> action) : this(key, key, shift, action)
            {
            }

            public bool Match(Keys key, bool shift)
            {
                return key >= StartKey && key <= EndKey && shift == Shift;
            }
        }

        public class ButtonInfo
        {
            public ConfigButton Config { get; set; }
            public Button Button { get; set; }
            public Keys Key => Config.Key;
            public bool Active { get; set; }
            public bool HotStandby { get; set; }

            public void Click()
            {
                Button.PerformClick();
            }

            public void ToggleStandby()
            {
                HotStandby = !HotStandby;
            }

            public void Recolor()
            {
                Button.BackColor = Active ? Color.Chartreuse : (HotStandby ? HotStandbyColor : Color.Transparent);
                Button.ForeColor = Active ? Color.Black : Color.White;
            }
        }

        private static readonly Color HotStandbyColor = Helper.Mix(Color.DodgerBlue, Color.FromArgb(51, 51, 51), 50);
        private readonly Font _regularFont;
        private readonly Font _boldFont;

        private readonly List<KeyMapEntry> _keyMap = new List<KeyMapEntry>();
        private readonly Config _config = new Config("midi-config.xml");
        private readonly SystemVolume _volume;
        private readonly List<ButtonInfo> _buttons = new List<ButtonInfo>();
        private readonly FormKeyboard _formKeyboard;
        private readonly FormSettings _formSettings;
        private readonly FormMessageTrace _formTracing;
        private bool _closing;
        private int _currentChannel;
        private bool _midiInitialized;
        private Keys? _activeHotKey;
        private DateTime _activeHotKeyTime;

        public FormMain()
        {
            InitializeComponent();

            _regularFont = new Font(label1.Font, FontStyle.Regular);
            _boldFont = new Font(label1.Font, FontStyle.Bold);

            label1.Text = "";
            _config.Updated += (sender, args) => InitializeFromConfig();

            _formSettings = new FormSettings(_config);
            _formTracing = new FormMessageTrace();
            _formKeyboard = new FormKeyboard();
            _volume = new SystemVolume();

            _keyMap.AddRange(new[]
            {
                // Space - toggle hot switch buttons
                new KeyMapEntry(Keys.Space, false, keys => PerformHotSwitch()),

                // Escape - reset everything
                new KeyMapEntry(Keys.Escape, false, keys => ResetAllNotes()),

                // Volume up/down
                new KeyMapEntry(Keys.Add, false, keys => _volume.Increase10()),
                new KeyMapEntry(Keys.Subtract, false, keys => _volume.Decrease10()),

                // Send test note
                new KeyMapEntry(Keys.Divide, false, keys => SendTestNotes()),

                // Function keys for loading and saving performances
                new KeyMapEntry(Keys.F1, Keys.F12, false, SelectPerformance),
                new KeyMapEntry(Keys.F1, Keys.F12, true, SavePerformance),

                // A-Z, 0-9 keys for switching instruments
                new KeyMapEntry(Keys.A, Keys.Z, false, keys => _buttons.FirstOrDefault(x => x.Key == keys)?.Click()),
                new KeyMapEntry(Keys.A, Keys.Z, true, keys => { _buttons.FirstOrDefault(x => x.Key == keys)?.ToggleStandby(); RecolorButtons(); }),
                new KeyMapEntry(Keys.D0, Keys.D9, false, keys => _buttons.FirstOrDefault(x => x.Key == keys)?.Click()),
                new KeyMapEntry(Keys.D0, Keys.D9, true, keys => { _buttons.FirstOrDefault(x => x.Key == keys)?.ToggleStandby(); RecolorButtons(); }),
            });

            RegisterAppBar();
        }

        private void PerformHotSwitch()
        {
            foreach (var b in _buttons.Where(x => x.HotStandby))
                b.Button.PerformClick();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            MidiDeviceManager.Shutdown();
            timer1.Enabled = false;

            _closing = true;
            _formSettings.Close();
            _formTracing.Close();
            _formKeyboard.Close();

            for (var i = 0; i < 10; i++)
            {
                Application.DoEvents();
                Thread.Sleep(10);
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            UnregisterAppBar();
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control || e.Alt)
                return;

            // Find key map entry
            var keymap = _keyMap.FirstOrDefault(x => x.Match(e.KeyCode, e.Shift));
            if (keymap == null)
                return;

            e.Handled = true;
            keymap.Action(e.KeyCode);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                VirtualMidi.CheckDriverVersion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection to the VirtualMIDI driver failed: " + ex.Message + "\r\n\r\n" +
                                "Please verify that VirtualMIDI is installed correctly. If it isn't, it can be " +
                                "downloaded from www.tobias-erichsen.de - look for the loopMIDI download.",
                  "Driver initialization failure", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Close();
            }

            InitializeFromConfig();
        }

        public void InitializeFromConfig()
        {
            if (_closing)
                return;

            // Set up input device
            midiInputBus.Text = _config.InputDevice;
            UpdatePerformances();

            if (!_midiInitialized)
            {
                // Create flow output devices
                var outputs = flowOutputDevicesPanel.Controls.OfType<MidiBus>().ToList();
                foreach (var output in outputs)
                    flowOutputDevicesPanel.Controls.Remove(output);

                var outputCount = _config.OutputDevices.Count;
                foreach (var output in _config.OutputDevices)
                {
                    var bus = new MidiBus
                    {
                        Title = output,
                        TitleColor = Color.Transparent,
                        ForeColor = Color.White,
                        BackColor = Color.Transparent,
                        Width = flowOutputDevicesPanel.Width,
                        Height = outputCount > 2 ? 20 : 30
                    };

                    flowOutputDevicesPanel.Controls.Add(bus);
                    flowOutputDevicesPanel.SetFlowBreak(bus, true);
                }
            }

            // Create flow buttons
            flowButtonPanel.Controls.Clear();

            var first = true;
            foreach (var c in _config.Buttons)
            {
                var bi = new ButtonInfo
                {
                    Button = new Button
                    {
                        Text = (c.Key == Keys.None ? "·" : Helper.KeyToString(c.Key)) + " " + c.Name,
                        BackColor = Color.FromArgb(51, 51, 51),
                        FlatStyle = FlatStyle.Flat,
                        AutoSize = true,
                        AutoSizeMode = AutoSizeMode.GrowAndShrink,
                        FlatAppearance = { BorderColor = Color.Gray, MouseOverBackColor = Color.DodgerBlue },
                        TabStop = false,
                        ForeColor = Color.White,
                    },
                    Config = c,
                    Active = first
                };
                bi.Button.Click += ChannelButtonClick;
                bi.Button.MouseDown += ChannelButtonMouseClick;

                _buttons.Add(bi);
                flowButtonPanel.Controls.Add(bi.Button);

                if (c.BreakAfter == BreakType.NewLine)
                    flowButtonPanel.SetFlowBreak(bi.Button, true);
                else if (c.BreakAfter == BreakType.Separator)
                    flowButtonPanel.Controls.Add(NewLabelSeparator(bi.Button.Height));

                first = false;
            }

            // Configure MIDI Device Manager
            if (!_midiInitialized)
            {
                MidiDeviceManager.SetOutputDevices(_config.OutputDevices);
                _midiInitialized = true;
            }

            MidiDeviceManager.RescanInputDevice(_config.InputDevice, DeviceMessage);
            midiBus1.Text = MidiDeviceManager.InputDevice?.Name ?? "No device found";

            SetCurrentChannel(_config.DefaultChannel);

            // Calculate new height
            var border = Height - flowOutputDevicesPanel.Height;
            Height = Math.Max(flowOutputDevicesPanel.PreferredSize.Height, flowButtonPanel.PreferredSize.Height) + border;
            ABSetPos();

            RecolorButtons();
            GC.Collect();
        }

        private Control NewLabelSeparator(int height)
        {
            return new Label
            {
                AutoSize = false,
                Text = ".",
                ForeColor = Color.White,
                Width = 16,
                Height = height,
                TextAlign = ContentAlignment.MiddleCenter
            };
        }

        private void ChannelButtonClick(object sender, EventArgs e)
        {
            var info = _buttons.FirstOrDefault(x => x.Button == sender);
            if (info == null)
                return;

            info.Active = !info.Active;
            if (!info.Active)
            {
                MidiDeviceManager.SendAllNotesOff(info.Config.Device, info.Config.Channel);
                _formKeyboard.Piano.ClearAllKeys();
            }

            RecolorButtons();
        }

        private void ChannelButtonMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            var info = _buttons.FirstOrDefault(x => x.Button == sender);
            if (info == null)
                return;

            info.HotStandby = !info.HotStandby;
            RecolorButtons();
        }

        private void CurrentChannelButtonClick(object sender, EventArgs e)
        {
            if (sender == midiLeft && _currentChannel > 0)
                SetCurrentChannel(_currentChannel - 1);
            else if (sender == midiRight && _currentChannel < 15)
                SetCurrentChannel(_currentChannel + 1);
        }

        private void DeviceMessage(object sender, MidiMessageEventArgs args)
        {
            var msg = args.Message;
            var inChannel = msg.Channel;
            var highlight = new List<Tuple<int, int>>();

            // Start by outputting the data - as low latency as possible
            if (msg.Channel == _currentChannel)
                foreach (var info in _buttons.Where(x => x.Active))
                {
                    msg.Channel = (byte)info.Config.Channel;
                    MidiDeviceManager.OutputDevices.ElementAtOrDefault(info.Config.Device)?.SendCommand(msg);
                    highlight.Add(new Tuple<int, int>(info.Config.Device, info.Config.Channel));
                }

            // Update UI
            midiInputBus.TriggerChannel(inChannel);
            foreach (var h in highlight)
                (flowOutputDevicesPanel.Controls[h.Item1] as MidiBus)?.TriggerChannel(h.Item2);

            if (highlight.Any())
            {
                // Update piano
                if (msg.IsNoteOffMessage)
                    _formKeyboard.Piano.ClearKey(msg.Data1);
                else if (msg.IsNoteOnMessage)
                    _formKeyboard.Piano.SetKeyDown(msg.Data1, msg.Data2);
            }

            // Add to FormTools
            msg.Channel = inChannel;
            _formTracing.AddMessage(msg);

            // Tick
            Invoke((Action) delegate
            {
                TimerTick(this, EventArgs.Empty);
            });
        }

        private void RecolorButtons()
        {
            Invoke((Action) delegate
            {
                _buttons.ForEach(x => x.Recolor());
            });
        }

        private void ResetAllNotes()
        {
            foreach (var button in _buttons)
            {
                button.Active = false;
                button.HotStandby = false;
            }

            MidiDeviceManager.SendAllNotesOff();
            _formKeyboard.Piano.ClearAllKeys();

            if (_buttons.Any())
                _buttons.First().Active = true;

            RecolorButtons();
        }

        private void SendTestNotes()
        {
            Task.Run(() =>
            {
                DeviceMessage(this, new MidiMessageEventArgs(0, MidiMessage.NoteOn(_currentChannel, 0x3C, 0x40)));
                Thread.Sleep(100);
                DeviceMessage(this, new MidiMessageEventArgs(0, MidiMessage.NoteOn(_currentChannel, 0x40, 0x50)));
                Thread.Sleep(100);
                DeviceMessage(this, new MidiMessageEventArgs(0, MidiMessage.NoteOn(_currentChannel, 0x43, 0x60)));
                Thread.Sleep(1000);

                DeviceMessage(this, new MidiMessageEventArgs(0, MidiMessage.NoteOff(_currentChannel, 0x3C, 0)));
                DeviceMessage(this, new MidiMessageEventArgs(0, MidiMessage.NoteOff(_currentChannel, 0x40, 0)));
                DeviceMessage(this, new MidiMessageEventArgs(0, MidiMessage.NoteOff(_currentChannel, 0x43, 0)));
            });
        }

        public void SetCurrentChannel(int channel)
        {
            _currentChannel = channel;
            labelCurrentChannel.Text = (channel + 1).ToString();
            midiInputBus.ActiveChannel = channel;
            midiInputBus.Invalidate();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (IsDisposed || _closing)
                return;

            try
            {
                var ticks = DateTime.Now.Millisecond;

                midiInputBus.Tick();
                foreach (var control in flowOutputDevicesPanel.Controls.OfType<MidiBus>())
                    control.Tick();

                // Update volume indicator
                var v = ((int)_volume.GetVolume());
                labelVolume.Text = v.ToString();
                progressBar1.Value = v;

                // Update window focus indicator
                var active = WinApi.GetForegroundWindow() == Handle;
                focusLabel.BackColor = active ? Color.Chartreuse : (ticks < 400 ? Color.Red : Color.Gray);

                // Update hot key indicator
                if (_activeHotKeyTime < DateTime.Now)
                {
                    _activeHotKeyTime = DateTime.MinValue;
                    _activeHotKey = null;
                    UpdatePerformances();
                }

                // Rescan MIDI output
                if (_closing)
                    return;

                MidiDeviceManager.RescanInputDevice(_config.InputDevice, DeviceMessage);
                midiInputBus.Title = MidiDeviceManager.InputDevice?.Name ?? _config.InputDevice;
                midiInputBus.TitleColor = MidiDeviceManager.ConnectedToInput ? Color.Transparent : (ticks < 700 ? Color.Red : Color.Transparent);
            }
            catch
            {
                //
            }
        }

        private void menuHamburger_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var pt = button.PointToScreen(new Point(button.Width - menuHamburger.Width, button.Height));

            menuHamburger.Show(pt);
            WinApi.SetWindowPos(menuHamburger.Handle, IntPtr.Zero, pt.X, pt.Y, 0, 0, WinApi.SWP.NOSIZE);
        }

        private void menuClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuSettings_Click(object sender, EventArgs e)
        {
            _formSettings.Show();
        }

        private void menuTools_Click(object sender, EventArgs e)
        {
            _formTracing.Show();
        }

        private void menuShowPiano_Click(object sender, EventArgs e)
        {
            _formKeyboard.Show();
        }

        private void aboutMIDIJunctionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormAbout())
                form.ShowDialog();
        }

        private void showOverviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.gefvert.org/site/downloads/midi-junction");
        }

        private void keysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var msg = new List<string>
            {
                "NUMPAD + -\tRaise/lower volume",
                "NUMPAD /\tSend test notes",
                "ESC\t\tAll notes off and reset instruments",
                "",
                "A-Z, 0-9\t\tActivate instrument",
                "SHIFT A-Z, 0-9\tMake instrument hot-standby",
                "SPACE\t\tToggle hot-standby instruments",
                "",
                "F1-F12\t\tSelect performance",
                "Shift F1-F12\tSave current instruments as performance"
            };

            MessageBox.Show(string.Join("\r\n", msg), "Keys", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void SavePerformance(Keys key)
        {
            var fkey = (int) key - (int) Keys.F1 + 1;
            if (fkey < 1 || fkey > 12)
                return;

            var items = _buttons
                .Where(button => button.Active || button.HotStandby)
                .Select(b => b.Config.Device + "," + b.Config.Channel + "," + (b.HotStandby ? "H" : "") + (b.Active ? "A" : ""))
                .ToList();

            var result = FormInputDialog.Execute("Save performance", "Save performance as...", _config.Performances.GetOrDefault(fkey)?.Title);
            if (result == null)
                return;

            if (result == "")
                _config.Performances.Remove(fkey);
            else
                _config.Performances[fkey] = new ConfigPerformance
                {
                    FKey = fkey,
                    Title = result,
                    Data = string.Join("|", items)
                };

            UpdatePerformances();
            _config.Save();
        }

        public void SelectPerformance(Keys key)
        {
            var data = _config.Performances.GetOrDefault((int)key - (int)Keys.F1 + 1)?.Data;
            if (string.IsNullOrEmpty(data))
            {
                // Not a valid hot key, disable any current indication
                _activeHotKey = null;
                _activeHotKeyTime = DateTime.MinValue;
                UpdatePerformances();
                return;
            }

            if (_activeHotKey != key)
            {
                // No double-press yet, select hot key and display indication
                _activeHotKey = key;
                _activeHotKeyTime = DateTime.Now.AddSeconds(1);
                UpdatePerformances();
                return;
            }

            // Double hot key press. Switch to new configuration.
            SelectPerformance(data);
            _activeHotKey = null;
            _activeHotKeyTime = DateTime.MinValue;
            UpdatePerformances();
        }

        public void SelectPerformance(string data)
        {
            // Reset all buttons
            foreach (var button in _buttons)
            {
                button.Active = false;
                button.HotStandby = false;
            }

            var items = data.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in items)
            {
                var subitems = item.Split(',');
                if (subitems.Length != 3)
                    continue;

                int device;
                int channel = -1;
                var success = int.TryParse(subitems[0], out device) && int.TryParse(subitems[1], out channel);
                if (!success)
                    continue;

                foreach (var button in _buttons.Where(x => x.Config.Device == device && x.Config.Channel == channel))
                {
                    button.Active = subitems[2].Contains("A");
                    button.HotStandby = subitems[2].Contains("H");

                    if (!button.Active)
                        MidiDeviceManager.SendAllNotesOff(button.Config.Device, button.Config.Channel);
                }
            }

            RecolorButtons();
        }

        public void UpdatePerformances()
        {
            if (_activeHotKey != null)
            {
                label1.Text = "     PRESS " + Helper.KeyToString(_activeHotKey.Value) + " AGAIN TO ACTIVATE     ";
                label1.Font = _boldFont;
                label1.ForeColor = Color.Black;
                label1.BackColor = Color.Chartreuse;
            }
            else
            {
                label1.Text = string.Join("   ·   ", _config.Performances.Values.Select(x => "F" + x.FKey + " " + x.Title));
                label1.Font = _regularFont;
                label1.ForeColor = Color.DarkGray;
                label1.BackColor = Color.Transparent;
            }
        }
    }
}
