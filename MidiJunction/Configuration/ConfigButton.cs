using System;
using System.Windows.Forms;

namespace MidiJunction.Configuration
{
    public enum BreakType
    {
        None,
        NewLine,
        Separator
    }

    public class ConfigButton
    {
        public int Device { get; set; }
        public int Channel { get; set; }
        public string Name { get; set; }
        public Keys Key { get; set; }
        public BreakType BreakAfter { get; set; }

        public ConfigButton Clone()
        {
            return (ConfigButton)MemberwiseClone();
        }
    }
}
