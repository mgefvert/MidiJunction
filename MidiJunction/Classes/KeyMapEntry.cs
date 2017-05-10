using System;
using System.Windows.Forms;

namespace MidiJunction.Classes
{
    public class KeyMapEntry
    {
        public Keys StartKey { get; }
        public Keys EndKey { get; }
        public bool Shift { get; }
        public Action<Keys> Action { get; }

        public KeyMapEntry(Keys startKey, Keys endKey, bool shift, Action<Keys> action)
        {
            StartKey = startKey;
            EndKey = endKey;
            Shift = shift;
            Action = action;
        }

        public KeyMapEntry(Keys key, bool shift, Action<Keys> action) : this(key, key, shift, action)
        {
        }

        public bool Match(Keys key, bool shift)
        {
            return key >= StartKey && key <= EndKey && shift == Shift;
        }
    }
}
