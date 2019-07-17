using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MidiJunction.Classes;
using MidiJunction.Configuration;
using MidiJunction.Controls;
using MidiJunction.Devices;

// ReSharper disable LocalizableElement

namespace MidiJunction.Forms
{
    public partial class FormMain : AppBarForm
    {
        private static readonly Color DarkGray = Color.FromArgb(51, 51, 51);

        private readonly Font _regularFont;
        private readonly Font _boldFont;

        private readonly List<KeyMapEntry> _keyMap = new List<KeyMapEntry>();
        private readonly Config _config = new Config("midi-config.xml");
        private readonly SystemVolume _volume;
        private readonly List<ButtonInfo> _buttons = new List<ButtonInfo>();
        private readonly FormKeyboard _formKeyboard;
        private readonly FormSettings _formSettings;
        private readonly FormMessageTrace _formTracing;
        private readonly FormSecondaryScreen _formSecondary;
        private readonly FormPerformances _formPerformances;
        private bool _closing;
        private bool _midiInitialized;
        private Keys? _activeHotKey;
        private DateTime _activeHotKeyTime;
        private DateTime _lastChordTime;
        private int _transpose;
        private readonly List<int> _hotStandby = new List<int>();

        public int CurrentChannel => NoteManager.CurrentChannel;
        private int _lastTime;

        public FormMain()
        {
            InitializeComponent();

            NoteManager.MidiMessage += ChannelDeviceMessage;

            _regularFont = new Font(label1.Font, FontStyle.Regular);
            _boldFont = new Font(label1.Font, FontStyle.Bold);

            labelChord.Text = "";
            label1.Text = "";

            _config.Changed += (sender, args) => InitializeFromConfig();
            _config.Performances.Changed += (sender, args) => PerformancesUpdate();

            _formSettings = new FormSettings(_config);
            _formTracing = new FormMessageTrace();
            _formKeyboard = new FormKeyboard();
            _formSecondary = new FormSecondaryScreen();
            _formPerformances = new FormPerformances(_config, _config.Performances);
            _volume = new SystemVolume();

            _keyMap.AddRange(new[]
            {
                // Space - toggle hot switch buttons
                new KeyMapEntry(Keys.Space, false, keys => StandbyPerformSwitch()),

                // Escape - reset everything
                new KeyMapEntry(Keys.Escape, false, keys => NotesResetAll()),

                // Volume up/down
                new KeyMapEntry(Keys.Add, false, keys => _volume.Increase10()),
                new KeyMapEntry(Keys.Subtract, false, keys => _volume.Decrease10()),

                // Volume up/down
                new KeyMapEntry(Keys.Up, false, keys => transposeUp.PerformClick()),
                new KeyMapEntry(Keys.Down, false, keys => transposeDown.PerformClick()),

                // Send test note
                new KeyMapEntry(Keys.Divide, false, keys => NotesSendTest()),

                // Function keys for loading and saving performances
                new KeyMapEntry(Keys.F1, Keys.F11, false, PerformancesSelect),
                new KeyMapEntry(Keys.F1, Keys.F11, true, PerformancesSave),
                new KeyMapEntry(Keys.F12, false, PerformancesView),

                // A-Z, 0-9 keys for switching instruments
                new KeyMapEntry(Keys.A, Keys.Z, false, keys => _buttons.FirstOrDefault(x => x.Key == keys)?.Click()),
                new KeyMapEntry(Keys.A, Keys.Z, true, keys => { StandbyToggle(_buttons.FirstOrDefault(x => x.Key == keys)); ButtonsRecolor(); }),
                new KeyMapEntry(Keys.D0, Keys.D9, false, keys => _buttons.FirstOrDefault(x => x.Key == keys)?.Click()),
                new KeyMapEntry(Keys.D0, Keys.D9, true, keys => { StandbyToggle(_buttons.FirstOrDefault(x => x.Key == keys)); ButtonsRecolor(); }),
            });

            RegisterAppBar();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WinApi.WM_POWERBROADCAST)
            {
                switch ((int)m.WParam)
                {
                    case WinApi.PBT_APMRESUMEAUTOMATIC:
                    case WinApi.PBT_APMRESUMESUSPEND:
                        MidiDeviceManager.RescanInputDevice(_config.InputDevice, ChannelDeviceMessage);
                        timer1.Enabled = true;
                        break;

                    case WinApi.PBT_APMSUSPEND:
                        timer1.Enabled = false;
                        break;
                }
            }
            else if (m.Msg == WinApi.WM_DISPLAYCHANGE)
            {
                MonitorsChanged();
            }

            base.WndProc(ref m);
        }

        private void MonitorsChanged()
        {
            // Very nasty hack. Screen.AllScreens is a static member initialized on first access, meaning in order to
            // make it update in response to display changes, we have to un-initialize the hidden array.
            var screenType = typeof(Screen);
            var screenField = screenType.GetField("screens", BindingFlags.Static | BindingFlags.NonPublic);
            if (screenField != null)
                screenField.SetValue(null, null);

            // Use secondary monitor?
            if (Screen.AllScreens.Length > 1 && _config.UseSecondaryMonitor)
            {
                var monitor = Screen.AllScreens.First(s => !s.Primary).Bounds;
                _formSecondary.Show();
                _formSecondary.SetBounds(monitor.X, monitor.Y, monitor.Width, monitor.Height, BoundsSpecified.All);
            }
            else
                _formSecondary.Hide();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            MidiDeviceManager.Shutdown();
            timer1.Enabled = false;
            _closing = true;

            // Give things a chance to settle down.
            Helper.WaitALittle(TimeSpan.FromMilliseconds(500));

            _formSettings.Close();
            _formTracing.Close();
            _formKeyboard.Close();
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

            SuspendLayout();
            try
            {
                // Set up input device
                midiInputBus.Text = _config.InputDevice;
                PerformancesUpdate();

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
                _buttons.Clear();
                foreach (var c in _config.Buttons)
                {
                    var bi = new ButtonInfo
                    {
                        Button = new Button
                        {
                            Text = (c.Key == Keys.None ? "·" : Helper.KeyToString(c.Key)) + " " + c.Name,
                            BackColor = DarkGray,
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

                MidiDeviceManager.RescanInputDevice(_config.InputDevice, ChannelDeviceMessage);
                midiBus1.Text = MidiDeviceManager.InputDevice?.Name ?? "No device found";

                ChannelSetCurrent(_config.DefaultChannel);

                ButtonsRecolor();
                MonitorsChanged();
            }
            finally
            {
                ResumeLayout(true);
            }

            // Calculate new height
            var border = Height - flowOutputDevicesPanel.Height;
            Height = Math.Max(flowOutputDevicesPanel.PreferredSize.Height, flowButtonPanel.PreferredSize.Height) + border;
            ABSetPos();

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

        private void ButtonsRecolor()
        {
            Invoke((Action) delegate
            {
                foreach(var button in _buttons)
                    button.Recolor();
            });
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

            ButtonsRecolor();
        }

        private void ChannelButtonMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            var info = _buttons.FirstOrDefault(x => x.Button == sender);
            if (info == null)
                return;

            StandbyToggle(info);
            ButtonsRecolor();
        }

        private void ChannelCurrentButtonClick(object sender, EventArgs e)
        {
            if (sender == midiLeft && CurrentChannel > 0)
                ChannelSetCurrent(CurrentChannel - 1);
            else if (sender == midiRight && CurrentChannel < 15)
                ChannelSetCurrent(CurrentChannel + 1);
        }

        private void ChannelDeviceMessage(object sender, MidiMessageEventArgs args)
        {
            var msg = args.Message;
            var inChannel = msg.Channel;
            var highlight = new List<Tuple<int, int>>();

            // Start by outputting the data - as low latency as possible
            if (msg.Channel == CurrentChannel)
            {
                // Check if it's a performance switch message - before transposing
                if (_config.LowKeysControlHotKeys && msg.IsNoteMessage && msg.Data1 >= 21 && msg.Data1 <= 24)
                {
                    if (!msg.IsNoteOnMessage)
                        return;

                    StandbyPerformSwitch(msg.Data1 - 21);
                    return;
                }

                // Check if it's a transposing switch message - before transposing
                if (_config.LowKeysControlHotKeys && msg.IsNoteMessage && (msg.Data1 == 107 || msg.Data1 == 108))
                {
                    if (!msg.IsNoteOnMessage)
                        return;

                    if (msg.Data1 == 107)
                        transposeDown_Click(this, EventArgs.Empty);
                    else if (msg.Data1 == 108)
                        transposeUp_Click(this, EventArgs.Empty);

                    return;
                }

                // Apply transposing
                if (msg.IsNoteMessage && _transpose != 0)
                {
                    var newNote = msg.Data1 + _transpose;
                    if (newNote < 1 || newNote > 127)
                        return;

                    msg.Data1 = (byte)newNote;
                }

                if (msg.IsNoteOnMessage)
                    NoteManager.Activate(msg.Data1);
                else if (msg.IsNoteOffMessage)
                    NoteManager.Deactivate(msg.Data1);

                var channels = msg.IsControlMessage && _config.ControlOnAllChannels
                    ? _buttons
                    : _buttons.Where(x => x.Active).ToList();

                foreach (var info in channels)
                {
                    msg.Channel = (byte) info.Config.Channel;
                    MidiDeviceManager.OutputDevices.ElementAtOrDefault(info.Config.Device)?.SendCommand(msg);
                    highlight.Add(new Tuple<int, int>(info.Config.Device, info.Config.Channel));
                }
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

        public void ChannelSetCurrent(int channel)
        {
            NoteManager.CurrentChannel = channel;
            labelCurrentChannel.Text = (channel + 1).ToString();
            midiInputBus.ActiveChannel = channel;
            midiInputBus.Invalidate();
        }

        private void NotesResetAll()
        {
            foreach (var button in _buttons)
            {
                button.Active = false;
                button.HotStandby = 0;
            }

            MidiDeviceManager.SendAllNotesOff();
            _formKeyboard.Piano.ClearAllKeys();

            ButtonsRecolor();
        }

        private void NotesSendTest()
        {
            Task.Run(() =>
            {
                NoteManager.TurnNoteOn(0x3C, 0x40);
                Thread.Sleep(100);
                NoteManager.TurnNoteOn(0x40, 0x50);
                Thread.Sleep(100);
                NoteManager.TurnNoteOn(0x43, 0x60);
                Thread.Sleep(1000);

                NoteManager.TurnAllNotesOff();
            });
        }

        private void menuClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuHamburger_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var pt = button.PointToScreen(new Point(button.Width - menuHamburger.Width, button.Height));

            menuHamburger.Show(pt);
            WinApi.SetWindowPos(menuHamburger.Handle, IntPtr.Zero, pt.X, pt.Y, 0, 0, WinApi.SWP.NOSIZE);
        }

        private void menuHelpAbout_Click(object sender, EventArgs e)
        {
            using (var form = new FormAbout())
                form.ShowDialog();
        }

        private void menuHelpKeys_Click(object sender, EventArgs e)
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
                "F1-F11\t\tSelect performance",
                "Shift F1-F11\tSave current instruments as performance",
                "F12\t\tDisplay performances window"
            };

            MessageBox.Show(string.Join("\r\n", msg), "Keys", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menuHelpOverview_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.gefvert.org/site/downloads/midi-junction");
        }

        private void menuPerformances_Click(object sender, EventArgs e)
        {
            _formPerformances.Show();
        }

        private void menuSettings_Click(object sender, EventArgs e)
        {
            _formSettings.Show();
        }

        private void menuShowPiano_Click(object sender, EventArgs e)
        {
            _formKeyboard.Show();
        }

        private void menuTools_Click(object sender, EventArgs e)
        {
            _formTracing.Show();
        }

        public void PerformancesSave(Keys key)
        {
            var items = _buttons
                .Where(button => button.Active || button.HotStandby != 0)
                .Select(b => b.Config.Device + "," + b.Config.Channel + "," + (b.Active ? "A" : "") + b.HotStandby)
                .ToList();

            var existingPerformance = _config.Performances.GetPerformance(key);
            var result = FormInputDialog.Execute("Save performance", "Save performance as...", existingPerformance?.Title);
            if (result == null)
                return;

            if (result == "")
                _config.Performances.RemovePerformance(key);
            else
                _config.Performances.SavePerformance(key, result, string.Join("|", items));

            PerformancesUpdate();
            _config.Save();
        }

        public void PerformancesSelect(Keys key)
        {
            var performance = _config.Performances.GetPerformance(key);
            if (performance == null)
            {
                // Not a valid hot key, disable any current indication
                _activeHotKey = null;
                _activeHotKeyTime = DateTime.MinValue;
                PerformancesUpdate();
                return;
            }

            if (_activeHotKey != key)
            {
                // No double-press yet, select hot key and display indication
                _activeHotKey = key;
                _activeHotKeyTime = DateTime.Now.AddSeconds(1);
                PerformancesUpdate();
                return;
            }

            // Double hot key press. Switch to new configuration.
            PerformancesSelect(performance.Data);
            _activeHotKey = null;
            _activeHotKeyTime = DateTime.MinValue;
            PerformancesUpdate();
        }

        public void PerformancesSelect(string data)
        {
            int ExtractStandbyNumber(string s)
            {
                var number = string.Join("", s.ToCharArray().Where(char.IsDigit));
                if (string.IsNullOrEmpty(number))
                    return 0;

                return int.TryParse(number, out var n)
                    ? Helper.Limit(n, 0, 3)
                    : 0;
            }

            // Reset all buttons
            foreach (var button in _buttons)
            {
                button.Active = false;
                button.HotStandby = 0;
            }

            var items = data.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in items)
            {
                var subitems = item.Split(',');
                if (subitems.Length != 3)
                    continue;

                var channel = -1;
                var success = int.TryParse(subitems[0], out int device) && int.TryParse(subitems[1], out channel);
                if (!success)
                    continue;

                foreach (var button in _buttons.Where(x => x.Config.Device == device && x.Config.Channel == channel))
                {
                    button.Active = subitems[2].Contains("A");
                    button.HotStandby = ExtractStandbyNumber(subitems[2]);

                    if (!button.Active)
                        MidiDeviceManager.SendAllNotesOff(button.Config.Device, button.Config.Channel);
                }
            }

            ButtonsRecolor();
        }

        public void PerformancesView(Keys key)
        {
            if (_formPerformances.Visible)
                _formPerformances.Hide();
            else
                _formPerformances.Show();
        }

        public void PerformancesUpdate()
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
                var performances = _config.Performances.Values
                    .Where(x => x.FKey != null)
                    .OrderBy(x => x.FKey)
                    .Select(x => $"F{x.FKey} {x.Title}")
                    .ToList();

                label1.Text = (performances.Any() ? string.Join("   ·   ", performances) : "No performances") +
                           @"           [ F12 Select ]";
                label1.Font = _regularFont;
                label1.ForeColor = Color.White;
                label1.BackColor = Color.Transparent;
            }
        }

        private void StandbyPerformSwitch()
        {
            var current = _hotStandby.Any() ? _hotStandby.Max() : 0;
            var max = _buttons.Max(x => x.HotStandby);

            if (current < max)
                StandbyPerformSwitch(current + 1);
            else
                StandbyPerformSwitch(0);
        }

        private void StandbyPerformSwitch(int standbyNote)
        {
            if (standbyNote == 0)
            {
                // All notes off
                _hotStandby.Clear();
                Invoke((Action) delegate
                {
                    foreach (var b in _buttons.Where(x => x.HotStandby > 0 && x.Active))
                        b.Button.PerformClick();
                });
                return;
            }

            if (_hotStandby.Contains(standbyNote))
                _hotStandby.Remove(standbyNote);
            else
                _hotStandby.Add(standbyNote);

            Invoke((Action)delegate
            {
                foreach (var b in _buttons.Where(x => x.HotStandby == standbyNote))
                    b.Button.PerformClick();
            });
        }

        public void StandbyToggle(ButtonInfo info)
        {
            if (info == null)
                return;

            // If the button has no standby, just increase to the first level.
            if (info.HotStandby == 0)
            {
                info.HotStandby = 1;
                return;
            }

            // Figure out the maximum standby level and how many buttons have that.
            var maxStandbyLevel = _buttons.Max(x => x.HotStandby);
            var maxStandbyCount = _buttons.Count(x => x.HotStandby == maxStandbyLevel);

            // Less than maximum?
            if (info.HotStandby < maxStandbyLevel)
            {
                info.HotStandby++;
                return;
            }

            // If we're already at maximum, or we're the only button at this level, wrap bac to zero.
            if (maxStandbyCount == 1 || maxStandbyLevel >= 3)
            {
                info.HotStandby = 0;
                return;
            }

            // We're not at maximum and there is another button with our level. Increase by one.
            info.HotStandby++;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (IsDisposed || _closing)
                return;

            timer1.Enabled = false;
            try
            {
                var ticks = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond % 900;
                var warnColor = ticks < 600 ? Color.Red : Color.Transparent;

                var chord = NoteManager.CurrentChord();
                if (chord != null)
                {
                    labelChord.Text = chord;
                    _lastChordTime = DateTime.Now;
                }
                else
                {
                    if ((DateTime.Now - _lastChordTime).TotalSeconds > 1)
                        labelChord.Text = "";
                }

                midiInputBus.Tick();
                foreach (var control in flowOutputDevicesPanel.Controls.OfType<MidiBus>())
                    control.Tick();

                // Update volume indicator
                var currentVolume = _volume.CurrentVolume;
                progressBar1.Value = currentVolume;

                if (!_volume.Mute)
                {
                    var vt = _volume.TargetVolume;
                    labelVolume.BackColor = Color.Transparent;
                    labelVolume.Text = vt == null ? "\xE2C1 " + currentVolume : currentVolume + "/" + vt;
                }
                else
                {
                    labelVolume.BackColor = warnColor;
                    labelVolume.Text = "MUTED";
                }

                // Update window focus indicator
                var active = WinApi.GetForegroundWindow() == Handle;
                focusLabel.BackColor = active ? Color.Chartreuse : warnColor;

                // Update hot key indicator
                if (_activeHotKeyTime < DateTime.Now)
                {
                    _activeHotKeyTime = DateTime.MinValue;
                    _activeHotKey = null;
                    PerformancesUpdate();
                }

                // Upate transpose feature
                if (_transpose == 0)
                {
                    labelTranspose.Text = "-";
                    labelTranspose.BackColor = Color.Transparent;
                }
                else
                {
                    labelTranspose.Text = (_transpose > 0 ? "+" : "-") + Math.Abs(_transpose);
                    labelTranspose.BackColor = ticks < 200 ? Color.Chartreuse : Color.Transparent;
                }

                // Only do this every 1/4 second
                var time = DateTime.Now.Millisecond / 250;
                if (_lastTime != time)
                {
                    _lastTime = time;

                    // Rescan MIDI output
                    MidiDeviceManager.RescanInputDevice(_config.InputDevice, ChannelDeviceMessage);

                    // Update instrument devices on secondary monitor
                    var devices = _buttons.Where(x => x.Active).Select(x => x.Config.Name).ToList();
                    _formSecondary.labelDevices.Text = string.Join("\r\n", devices);
                    _formSecondary.labelChord.Text = labelChord.Text;
                }

                midiInputBus.Title = MidiDeviceManager.InputDevice?.Name ?? _config.InputDevice;
                midiInputBus.TitleColor = MidiDeviceManager.ConnectedToInput ? Color.Transparent : warnColor;
            }
            catch
            {
                //
            }
            finally
            {
                timer1.Enabled = true;
            }
        }

        private void transposeUp_Click(object sender, EventArgs e)
        {
            _transpose = Math.Min(99, _transpose + 1);
            NoteManager.TurnAllNotesOff();
        }

        private void transposeDown_Click(object sender, EventArgs e)
        {
            _transpose = Math.Max(-99, _transpose - 1);
            NoteManager.TurnAllNotesOff();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down)
            {
                FormMain_KeyDown(this, new KeyEventArgs(keyData));
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
