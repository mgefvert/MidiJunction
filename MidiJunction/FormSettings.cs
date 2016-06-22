using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using MidiJunction.Devices;

namespace MidiJunction
{
  public partial class FormSettings : Form
  {
    private readonly Config _config;
    private readonly Dictionary<int, RadioButton> _breakAfterButtons;

    public FormSettings(Config config)
    {
      InitializeComponent();
      _config = config;

      _breakAfterButtons = new Dictionary<int, RadioButton>
      {
        { 1, breakAfter1 },
        { 2, breakAfter2 },
        { 3, breakAfter3 },
        { 4, breakAfter4 },
        { 5, breakAfter5 },
        { 6, breakAfter6 },
        { 7, breakAfter7 },
        { 8, breakAfter8 },
        { 9, breakAfter9 },
        { 10, breakAfter10 },
        { 11, breakAfter11 }
      };
    }

    private void label25_Click(object sender, EventArgs e)
    {
      Process.Start("http://www.gefvert.org/midi-junction");
    }

    private void label26_Click(object sender, EventArgs e)
    {
      Process.Start("http://www.tobias-erichsen.de/");
    }

    private void FormSettings_Shown(object sender, EventArgs e)
    {
      midiChannel.Value = Helper.Limit(_config.MidiDefaultChannel, 1, 16);
      inputDevice.Text = _config.MidiInputDevice;
      outputDevice.Text = _config.MidiOutputName;

      inputDevice.Items.Clear();
      for (int i = 0; i < Midi.InDeviceCount; i++)
      {
        var info = Midi.InDeviceInfo(i);
        var name = (info.szPname ?? "").Trim();
        if (!string.IsNullOrEmpty(name) && name != MidiDeviceManager.OutputDevice?.Name)
          inputDevice.Items.Add(name);
      }

      channel1.Text = _config.Channels[0];
      channel2.Text = _config.Channels[1];
      channel3.Text = _config.Channels[2];
      channel4.Text = _config.Channels[3];
      channel5.Text = _config.Channels[4];
      channel6.Text = _config.Channels[5];
      channel7.Text = _config.Channels[6];
      channel8.Text = _config.Channels[7];
      channel9.Text = _config.Channels[8];
      channel10.Text = _config.Channels[9];
      channel11.Text = _config.Channels[10];
      channel12.Text = _config.Channels[11];

      (_breakAfterButtons.ContainsKey(_config.BreakAfter) ? _breakAfterButtons[_config.BreakAfter] : breakAfterNone)
          .Checked = true;
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
      Hide();
    }

    private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing)
      {
        e.Cancel = true;
        Hide();
      }
    }

    private void okButton_Click(object sender, EventArgs e)
    {
      var outputText = outputDevice.Text.Trim();
      if (string.IsNullOrEmpty(outputText))
        outputText = "MIDI Junction";

      _config.MidiDefaultChannel = (int)midiChannel.Value;
      _config.MidiInputDevice = inputDevice.Text.Trim();
      _config.MidiOutputName = outputText;

      _config.Channels[0] = channel1.Text.Trim();
      _config.Channels[1] = channel2.Text.Trim();
      _config.Channels[2] = channel3.Text.Trim();
      _config.Channels[3] = channel4.Text.Trim();
      _config.Channels[4] = channel5.Text.Trim();
      _config.Channels[5] = channel6.Text.Trim();
      _config.Channels[6] = channel7.Text.Trim();
      _config.Channels[7] = channel8.Text.Trim();
      _config.Channels[8] = channel9.Text.Trim();
      _config.Channels[9] = channel10.Text.Trim();
      _config.Channels[10] = channel11.Text.Trim();
      _config.Channels[11] = channel12.Text.Trim();

      var breakButton = _breakAfterButtons.FirstOrDefault(x => x.Value.Checked);
      _config.BreakAfter = breakButton.Equals(default(KeyValuePair<int, RadioButton>)) ? 0 : breakButton.Key;

      _config.TriggerUpdated();
      Hide();
    }
  }
}
