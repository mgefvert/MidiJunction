using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MidiJunction.Devices;

// ReSharper disable LocalizableElement

namespace MidiJunction
{
  public partial class FormMain : AppBarForm
  {
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    public class ButtonInfo
    {
      public Button Button { get; set; }
      public Label MidiLabel { get; set; }
      public Keys Key { get; set; }
      public int Channel { get; set; }
      public bool Active { get; set; }
      public int Opacity { get; set; }

      public void Recolor()
      {
        Button.BackColor = Active ? Color.Chartreuse : Color.Transparent;
        Button.ForeColor = Active ? Color.Black : Color.White;
      }
    }

    private readonly Config _config = new Config("midi.ini");
    private readonly SystemVolume _volume;
    private bool _closing;
    private int _opacity;
    private readonly List<ButtonInfo> _buttons;
    private readonly FormKeyboard _formKeyboard;
    private readonly FormSettings _formSettings;
    private readonly FormTools _formTools;

    public int CurrentChannel { get; set; } = 1;

    public FormMain()
    {
      InitializeComponent();

      _config.Updated += OnConfigUpdated;

      _formSettings = new FormSettings(_config);
      _formTools = new FormTools();
      _formKeyboard = new FormKeyboard();

      _volume = new SystemVolume();
      _buttons = new[]
      {
        new ButtonInfo { Button = button1, MidiLabel = midi1, Channel = 0, Key = Keys.F1, Active = true },
        new ButtonInfo { Button = button2, MidiLabel = midi2, Channel = 1, Key = Keys.F2 },
        new ButtonInfo { Button = button3, MidiLabel = midi3, Channel = 2, Key = Keys.F3 },
        new ButtonInfo { Button = button4, MidiLabel = midi4, Channel = 3, Key = Keys.F4 },
        new ButtonInfo { Button = button5, MidiLabel = midi5, Channel = 4, Key = Keys.F5 },
        new ButtonInfo { Button = button6, MidiLabel = midi6, Channel = 5, Key = Keys.F6 },
        new ButtonInfo { Button = button7, MidiLabel = midi7, Channel = 6, Key = Keys.F7 },
        new ButtonInfo { Button = button8, MidiLabel = midi8, Channel = 7, Key = Keys.F8 },
        new ButtonInfo { Button = button9, MidiLabel = midi9, Channel = 8, Key = Keys.F9 },
        new ButtonInfo { Button = button10, MidiLabel = midi10, Channel = 9, Key = Keys.F10 },
        new ButtonInfo { Button = button11, MidiLabel = midi11, Channel = 10, Key = Keys.F11 },
        new ButtonInfo { Button = button12, MidiLabel = midi12, Channel = 11, Key = Keys.F12 },
      }.ToList();

      label2.Text = _config.MidiOutputName;

      RegisterAppBar();
    }

    private void OnConfigUpdated(object sender, EventArgs args)
    {
      FormMain_Load(this, EventArgs.Empty);
    }

    private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
    {
      MidiDeviceManager.Shutdown();
      timer1.Enabled = false;
      _closing = true;
      _config.Updated -= OnConfigUpdated;

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
        case Keys.Escape:
          button13.PerformClick();
          return;
        case Keys.Add:
          _volume.Increase10();
          return;
        case Keys.Subtract:
          _volume.Decrease10();
          return;
        default:
          _buttons.FirstOrDefault(x => x.Key == e.KeyCode)?.Button.PerformClick();
          return;
      }
    }

    private void FormMain_Load(object sender, EventArgs e)
    {
      label1.Text = "...";

      foreach (var info in _buttons)
      {
        var text = _config.Channels[info.Channel];
        if (!string.IsNullOrEmpty(text))
          text = "F" + (info.Channel + 1) + " " + text;

        info.Button.Text = text;
        flowLayoutPanel1.SetFlowBreak(info.Button, info.Channel + 1 == _config.BreakAfter);
      }

      RecolorButtons();

      if (MidiDeviceManager.OutputDevice == null)
        MidiDeviceManager.CreateOutputDevice(_config.MidiOutputName);

      MidiDeviceManager.RescanInputDevice(_config.MidiInputDevice, InDeviceOnMessage);
      label1.Text = MidiDeviceManager.InputDevice.Name;

      CurrentChannel = _config.MidiDefaultChannel;
      midiLabel.Text = CurrentChannel.ToString();

      GC.Collect();
    }

    private void InDeviceOnMessage(object sender, MidiMessageEventArgs args)
    {
      var msg = args.Message;
      var any = false;

      // Start by outputting the data - as low latency as possible
      if (msg.Channel + 1 == CurrentChannel)
        foreach (var info in _buttons.Where(x => x.Active))
        {
          msg.Channel = (byte)info.Channel;
          MidiDeviceManager.OutputDevice.SendCommand(msg.Bytes);
          any = true;
        }

      // Update UI
      _buttons[msg.Channel].Opacity = 100;
      _buttons[msg.Channel].MidiLabel.BackColor = Color.Chartreuse;

      if (any)
      {
        // Update arrow
        _opacity = 100;

        // Update piano
        if (msg.IsNoteOffMessage)
          _formKeyboard.Piano.ClearKey(msg.Data1);
        else if (msg.IsNoteOnMessage)
          _formKeyboard.Piano.SetKeyDown(msg.Data1, msg.Data2);
      }

      // Add to FormTools
      _formTools.AddMessage(msg);

      // Tick
      Invoke((Action)delegate { timer1_Tick(this, EventArgs.Empty); });
    }

    private void RecolorButtons()
    {
      Invoke((Action)delegate { _buttons.ForEach(x => x.Recolor()); });
    }

    private void channelButton_Click(object sender, EventArgs e)
    {
      var info = _buttons.FirstOrDefault(x => x.Button == sender);
      if (info == null)
        return;

      info.Active = !info.Active;
      if (!info.Active)
      {
        MidiDeviceManager.OutputDevice.SendAllNotesOff(info.Channel);
        _formKeyboard.Piano.ClearAllKeys();
      }

      RecolorButtons();
    }

    private void midiLeft_Click(object sender, EventArgs e)
    {
      if (sender == midiLeft && CurrentChannel > 1)
        CurrentChannel--;
      else if (sender == midiRight && CurrentChannel < 16)
        CurrentChannel++;

      midiLabel.Text = CurrentChannel.ToString();
    }

    private void pictureBox1_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void resetButton_Click(object sender, EventArgs e)
    {
      foreach (var button in _buttons)
        button.Active = false;

      MidiDeviceManager.OutputDevice.SendAllNotesOff();
      _formKeyboard.Piano.ClearAllKeys();
      RecolorButtons();
    }

    private void testButton_Click(object sender, EventArgs e)
    {
      Task.Run(() =>
      {
        var ch = CurrentChannel - 1;

        InDeviceOnMessage(this, new MidiMessageEventArgs(0, MidiMessage.NoteOn(ch, 0x3C, 0x40)));
        Thread.Sleep(100);
        InDeviceOnMessage(this, new MidiMessageEventArgs(0, MidiMessage.NoteOn(ch, 0x40, 0x60)));
        Thread.Sleep(100);
        InDeviceOnMessage(this, new MidiMessageEventArgs(0, MidiMessage.NoteOn(ch, 0x43, 0x70)));
        Thread.Sleep(1000);

        InDeviceOnMessage(this, new MidiMessageEventArgs(0, MidiMessage.NoteOff(ch, 0x3C, 0)));
        InDeviceOnMessage(this, new MidiMessageEventArgs(0, MidiMessage.NoteOff(ch, 0x40, 0)));
        InDeviceOnMessage(this, new MidiMessageEventArgs(0, MidiMessage.NoteOff(ch, 0x43, 0)));
      });
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      if (IsDisposed || _closing)
        return;

      try
      {
        var ticks = DateTime.Now.Millisecond;

        // Update main opacity arrow
        if (_opacity > 0)
        {
          _opacity = Math.Max(_opacity - 5, 0);
          label3.ForeColor = Mix(Color.Chartreuse, Color.DimGray, _opacity);
        }

        // Update individual opacity indicators
        foreach (var button in _buttons.Where(x => x.Opacity != 0))
        {
          button.Opacity = Math.Max(button.Opacity - 5, 0);
          button.MidiLabel.BackColor = Mix(Color.Chartreuse, Color.FromArgb(51, 51, 51), button.Opacity);
        }

        // Update volume indicator
        var v = _volume.GetVolume();
        progressBar1.Value = v;
        labelVolume.Text = v.ToString();

        // Update window focus indicator
        var active = GetForegroundWindow() == Handle;
        focusLabel.BackColor = active
            ? Color.Chartreuse
            : (ticks < 400 ? Color.Red : Color.Gray);

        // Rescan MIDI output
        if (_closing)
          return;

        MidiDeviceManager.RescanInputDevice(_config.MidiInputDevice, InDeviceOnMessage);
        label1.Text = MidiDeviceManager.InputDevice.Name;
        label1.BackColor = MidiDeviceManager.ConnectedToInput
            ? label2.BackColor
            : (ticks < 700 ? Color.Red : label2.BackColor);
      }
      catch
      {
        //
      }
    }

    private Color Mix(Color highlightColor, Color baseColor, int amount)
    {
      var a = amount / 100.0;
      return Color.FromArgb(
          (int)(highlightColor.R * a + baseColor.R * (1 - a)),
          (int)(highlightColor.G * a + baseColor.G * (1 - a)),
          (int)(highlightColor.B * a + baseColor.B * (1 - a))
      );
    }

    private void button14_Click(object sender, EventArgs e)
    {
      _formSettings.Show();
    }

    private void button16_Click(object sender, EventArgs e)
    {
      _formTools.Show();
    }

    private void button17_Click(object sender, EventArgs e)
    {
      _formKeyboard.Show();
    }
  }
}
