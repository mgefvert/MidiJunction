﻿using System;
using System.Windows.Forms;

namespace MidiJunction.Forms
{
  public partial class FormSettingButton : Form
  {
    public ConfigButton Button { get; set; }

    public FormSettingButton()
    {
      InitializeComponent();
    }

    public bool Execute()
    {
      ActiveControl = textBox1;
      textBox1.Text = Button.Name;
      comboBox1.SelectedIndex = Button.Device;
      comboBox2.SelectedIndex = Button.Channel;

      if (ShowDialog() != DialogResult.OK)
        return false;

      Button.Name = textBox1.Text.Trim();
      Button.Device = comboBox1.SelectedIndex;
      Button.Channel = comboBox2.SelectedIndex;
      return true;
    }
  }
}