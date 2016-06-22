using System;
using System.Collections.Generic;
using System.IO;

namespace MidiJunction
{
  public class Config
  {
    private readonly string _filename;

    public string MidiInputDevice { get; set; }
    public string MidiOutputName { get; set; }
    public string[] Channels { get; }
    public int MidiDefaultChannel { get; set; }
    public int BreakAfter { get; set; }

    public event EventHandler Updated;

    public Config(string filename)
    {
      Channels = new string[16];

      _filename = filename;
      Load();
    }

    private static string GetValue(Dictionary<string, string> dictionary, string key)
    {
      string result;
      return dictionary.TryGetValue(key, out result) ? result.Trim() : null;
    }

    public void Load()
    {
      var lines = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

      using (var fs = new FileStream(_filename, FileMode.OpenOrCreate))
      using (var reader = new StreamReader(fs))
      {
        while (!reader.EndOfStream)
        {
          var s = (reader.ReadLine() ?? "").Trim();
          if (string.IsNullOrEmpty(s))
            continue;

          var fields = s.Split('=');
          if (fields.Length != 2)
            continue;

          lines[fields[0].Trim()] = fields[1].Trim();
        }
      }

      for (int i = 0; i < 16; i++)
        Channels[i] = GetValue(lines, "Channel" + (i + 1));

      MidiInputDevice = GetValue(lines, "MidiInputDevice");
      MidiOutputName = GetValue(lines, "MidiOutputName");

      int n;
      int.TryParse(GetValue(lines, "BreakAfter"), out n);
      BreakAfter = n;

      MidiDefaultChannel = 1;
      if (int.TryParse(GetValue(lines, "MidiDefaultChannel"), out n))
        MidiDefaultChannel = Helper.Limit(n, 1, 16);
    }

    public void Save()
    {
      using (var fs = new FileStream(_filename, FileMode.Create))
      using (var writer = new StreamWriter(fs))
      {
        writer.WriteLine("MidiInputDevice = " + MidiInputDevice);
        writer.WriteLine("MidiOutputName = " + MidiOutputName);
        writer.WriteLine("MidiDefaultChannel = " + MidiDefaultChannel);
        writer.WriteLine();

        for (int i = 0; i < 16; i++)
          writer.WriteLine("Channel" + (i + 1) + " = " + Channels[i]);

        writer.WriteLine("BreakAfter = " + BreakAfter);
      }
    }

    public void TriggerUpdated()
    {
      Save();
      Updated?.Invoke(this, EventArgs.Empty);
    }
  }
}
