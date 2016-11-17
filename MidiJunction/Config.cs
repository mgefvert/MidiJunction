using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CommonNetTools;

namespace MidiJunction
{
    public class Config
    {
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

                Buttons.Add(new ConfigButton { Device = device, Channel = channel, Name = name });
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
              new XAttribute("name", x.Name)
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
