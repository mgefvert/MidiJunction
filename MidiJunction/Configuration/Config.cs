using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using DotNetCommons;
using MidiJunction.Classes;

namespace MidiJunction.Configuration
{
    public class Config
    {
        public event EventHandler Changed;

        public static readonly Keys[] AllowedKeys =
        {
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
        public bool ControlOnAllChannels { get; set; }
        public bool LowKeysControlHotKeys { get; set; } = true;
        public bool UseSecondaryMonitor { get; set; }
        public ConfigPerformances Performances = new ConfigPerformances();

        public Config(string filename)
        {
            InputDevice = null;
            OutputDevices.Add("MIDI Junction");
            DefaultChannel = 1;

            _filename = filename;
            Load();
        }

        protected void FireChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
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
                    Key = Helper.StringToKey(node.Attribute("key")?.Value),
                    BreakAfter = Helper.StringToEnum<BreakType>(node.Attribute("break")?.Value)
                });
            }

            Performances.Clear();
            foreach (var node in xml.Elements("performances").Elements("performance"))
            {
                var fkey = node.Attribute("fkey")?.Value.ParseInt();
                var title = node.Attribute("title")?.Value;
                var data = node.Attribute("data")?.Value;
                if (string.IsNullOrEmpty(title) || fkey < 1 || fkey > 12 || string.IsNullOrEmpty(data))
                    continue;

                Performances[title] = new ConfigPerformance
                {
                    FKey = fkey,
                    Title = title,
                    Data = data
                };
            }

            DefaultChannel = Helper.Limit(xml.Element("default-channel")?.Value.ParseInt() ?? 0, 0, 15);
            ControlOnAllChannels = (xml.Element("control-all-channels")?.Value.ParseInt() ?? 0) != 0;
            LowKeysControlHotKeys = (xml.Element("low-keys-control-hotkeys")?.Value.ParseInt() ?? 1) != 0;
            UseSecondaryMonitor = (xml.Element("use-secondary-monitor")?.Value.ParseInt() ?? 1) != 0;

            FireChanged();
        }

        public void Save()
        {
            var outputDevices = OutputDevices.Select(x => new XElement("device", new XAttribute("name", x)));
            var buttons = Buttons.Select(x => new XElement("button",
                new XAttribute("device", x.Device),
                new XAttribute("channel", x.Channel),
                new XAttribute("name", x.Name),
                new XAttribute("key", Helper.KeyToString(x.Key)),
                new XAttribute("break", x.BreakAfter.ToString())
            ));
            var performances = Performances.Values.Select(x => new XElement("performance",
                new XAttribute("title", x.Title),
                new XAttribute("data", x.Data),
                x.FKey != null ? new XAttribute("fkey", x.FKey) : null
            ));

            var doc = new XDocument(
                new XElement("xml",
                    new XElement("input-device", InputDevice),
                    new XElement("output-devices", outputDevices),
                    new XElement("default-channel", DefaultChannel),
                    new XElement("buttons", buttons),
                    new XElement("performances", performances),
                    new XElement("control-all-channels", ControlOnAllChannels ? 1 : 0),
                    new XElement("low-keys-control-hotkeys", LowKeysControlHotKeys ? 1 : 0),
                    new XElement("use-secondary-monitor", UseSecondaryMonitor ? 1 : 0)
                )
            );

            doc.Save(_filename);
        }

        public void TriggerUpdated()
        {
            Save();
            FireChanged();
        }
    }
}
