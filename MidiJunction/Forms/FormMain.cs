using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gefvert.Tools.Common;
using MidiJunction.Controls;
using MidiJunction.Devices;

// ReSharper disable LocalizableElement

namespace MidiJunction.Forms
{
  public partial class FormMain : AppBarForm
  {
    private static readonly Color HotStandbyColor = Helper.Mix(Color.DodgerBlue, Color.FromArgb(51, 51, 51), 50);

    public class ButtonInfo
    {
      public ConfigButton Config { get; set; }
      public Button Button { get; set; }
      public Keys Key { get; set; }
      public bool Active { get; set; }
      public bool HotStandby { get; set; }

      public void Recolor()
      {
        Button.BackColor = Active ? Color.Chartreuse : (HotStandby ? HotStandbyColor : Color.Transparent);
        Button.ForeColor = Active ? Color.Black : Color.White;
      }
    }

    private readonly Config _config = new Config("midi-config.xml");
    private readonly SystemVolume _volume;
    private bool _closing;
    private readonly List<ButtonInfo> _buttons = new List<ButtonInfo>();
    private readonly FormKeyboard _formKeyboard;
    private readonly FormSettings _formSettings;
    private readonly FormTools _formTools;
    private int _currentChannel;

    public FormMain()
    {
      InitializeComponent();

      _config.Updated += (sender, args) => InitializeFromConfig();

      _formSettings = new FormSettings(_config);
      _formTools = new FormTools();
      _formKeyboard = new FormKeyboard();
      _volume = new SystemVolume();

      RegisterAppBar();
    }

    private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
    {
      MidiDeviceManager.Shutdown();
      timer1.Enabled = false;

      _closing = true;
      _formSettings.Close();
      _formTools.Close();
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
      switch (e.KeyCode)
      {
        case Keys.Space:
          foreach(var button in _buttons.Where(x => x.HotStandby))
            button.Button.PerformClick();
          e.Handled = true;
          break;
        case Keys.Escape:
          ResetAllNotes();
          e.Handled = true;
          return;
        case Keys.Add:
          _volume.Increase10();
          e.Handled = true;
          return;
        case Keys.Subtract:
          _volume.Decrease10();
          e.Handled = true;
          return;
      }

      var bi = _buttons.FirstOrDefault(x => x.Key == e.KeyCode);
      if (bi == null)
        return;

      e.Handled = true;
      if (e.Shift)
      {
        bi.HotStandby = !bi.HotStandby;
        RecolorButtons();
      }
      else
        bi.Button.PerformClick();
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

      // Create flow output devices
      var outputs = flowOutputDevicesPanel.Controls.OfType<MidiBus>().ToList();
      foreach(var output in outputs)
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

      // Create flow buttons
      flowButtonPanel.Controls.Clear();

      var fKey = 1;
      var keys = new List<Keys> { Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12 };
      var first = true;
      foreach (var c in _config.Buttons)
      {
        var bi = new ButtonInfo
        {
          Button = new Button
          {
            Text = "F" + (fKey++) + " " + c.Name,
            BackColor = Color.FromArgb(51, 51, 51),
            FlatStyle = FlatStyle.Flat,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            FlatAppearance = { BorderColor = Color.Gray, MouseOverBackColor = Color.DodgerBlue },
            TabStop = false,
            ForeColor = Color.White,
          },
          Config = c,
          Key = keys.ExtractFirstOrDefault(),
          Active = first
        };
        bi.Button.Click += ChannelButtonClick;
        bi.Button.MouseDown += ChannelButtonMouseClick;

        _buttons.Add(bi);
        flowButtonPanel.Controls.Add(bi.Button);
        first = false;
      }

      foreach (var brk in _config.BreakAfter)
        flowButtonPanel.SetFlowBreak(flowButtonPanel.Controls[brk], true);

      // Configure MIDI Device Manager
      MidiDeviceManager.SetOutputDevices(_config.OutputDevices);
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
      _formTools.AddMessage(msg);

      // Tick
      Invoke((Action)delegate { TimerTick(this, EventArgs.Empty); });
    }

    private void RecolorButtons()
    {
      Invoke((Action)delegate { _buttons.ForEach(x => x.Recolor()); });
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
        foreach(var control in flowOutputDevicesPanel.Controls.OfType<MidiBus>())
          control.Tick();

        // Update volume indicator
        var v = ((int) _volume.GetVolume());
        labelVolume.Text = v.ToString();
        progressBar1.Value = v;

        // Update window focus indicator
        var active = WinApi.GetForegroundWindow() == Handle;
        focusLabel.BackColor = active ? Color.Chartreuse : (ticks < 400 ? Color.Red : Color.Gray);

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
      _formTools.Show();
    }

    private void menuShowPiano_Click(object sender, EventArgs e)
    {
      _formKeyboard.Show();
    }
  }
}
