using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using CommonNetTools;

namespace MidiJunction
{
    public class Config
    {
        public static readonly Keys[] AllowedKeys =
        {
            Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12,
            Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0,
            Keys.Q, Keys.W, Keys.E, Keys.R, Keys.T, Keys.Y, Keys.U, Keys.I, Keys.O, Keys.P,
            Keys.A, Keys.S, Keys.D, Keys.F, Keys.G, Keys.H, Keys.J, Keys.K, Keys.L,
            Keys.Z, Keys.X, Keys.C, Keys.V, Keys.B, Keys.N, Keys.M,
        };

        private readonly string _filename;

        public string InputDevice { get; set; }
        public List<string> OutputDevices { get; } = new List<string>();
        public List<ConfigButton> Buttons { get; } = new List<ConfigButton>();
        public int DefaultChannel { get; set; }
        public List<int> BreakAfter { get; } = new List<int>();

        public event EventHandler Updated;

        public Config(string filename)
        {
            InputDevice = null;
            OutputDevices.Add("MIDI Junction");
            DefaultChannel = 1;

            _filename = filename;
            Load();
        }

        public void Load()
        {
            var doc = XDocument.Load(_filename);

            var xml = doc.Root;
            if (xml == null)
                return;

            InputDevice = xml.Element("input-device")?.Value;

            OutputDevices.Clear();
            OutputDevices.AddRange(xml
              .Elements("output-devices")
              .Elements("device")
              .Select(x => x.Attribute("name")?.Value)
              .Where(x => !string.IsNullOrEmpty(x))
            );

            Buttons.Clear();
            foreach (var node in xml.Elements("buttons").Elements("button"))
            {
                var device = node.Attribute("device")?.Value.ParseInt(-1) ?? -1;
                var channel = node.Attribute("channel")?.Value.ParseInt(-1) ?? -1;
                if (device == -1 || channel == -1)
                    continue;

                var name = node.Attribute("name")?.Value;
                if (string.IsNullOrEmpty(name))
                    name = $"D{device} Ch{channel}";

                Buttons.Add(new ConfigButton
                {
                    Device = device,
                    Channel = channel,
                    Name = name,
                    Key = Helper.StringToKey(node.Attribute("key")?.Value)
                });
            }

            BreakAfter.Clear();
            BreakAfter.AddRange(xml
              .Elements("break")
              .Elements("after")
              .Select(x => x.Value.ParseInt())
              .Where(x => x != 0)
            );

            DefaultChannel = Helper.Limit(xml.Element("default-channel")?.Value.ParseInt() ?? 0, 0, 15);
        }

        public void Save()
        {
            var outputDevices = OutputDevices.Select(x => new XElement("device", new XAttribute("name", x)));
            var buttons = Buttons.Select(x => new XElement("button",
              new XAttribute("device", x.Device),
              new XAttribute("channel", x.Channel),
              new XAttribute("name", x.Name),
              new XAttribute("key", Helper.KeyToString(x.Key))
            ));
            var breaks = BreakAfter.Select(x => new XElement("after", x));

            var doc = new XDocument(
              new XElement("xml",
                new XElement("input-device", InputDevice),
                new XElement("output-devices", outputDevices),
                new XElement("default-channel", DefaultChannel),
                new XElement("buttons", buttons),
                new XElement("break", breaks)
              )
            );

            doc.Save(_filename);
        }

        public void TriggerUpdated()
        {
            Save();
            Updated?.Invoke(this, EventArgs.Empty);
        }
    }
}
