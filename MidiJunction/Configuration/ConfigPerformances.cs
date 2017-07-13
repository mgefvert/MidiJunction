using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

// ReSharper disable LocalizableElement

namespace MidiJunction.Configuration
{
    public class ConfigPerformances : Dictionary<string, ConfigPerformance>
    {
        public event EventHandler Changed;

        public ConfigPerformances() : base(StringComparer.CurrentCultureIgnoreCase)
        {
        }

        protected void FireChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private int FKeyToInt(Keys key)
        {
            var fkey = (int)key - (int)System.Windows.Forms.Keys.F1 + 1;
            return fkey >= 1 && fkey <= 11
                ? fkey
                : throw new ArgumentOutOfRangeException(nameof(key), "Key must be between F1 and F11.");
        }

        public void ClearPerformanceKey(Keys key)
        {
            var fkey = FKeyToInt(key);

            var changed = false;
            foreach (var performance in Values.Where(x => x.FKey == fkey))
            {
                performance.FKey = null;
                changed = true;
            }

            if (changed)
                FireChanged();
        }

        public ConfigPerformance GetPerformance(Keys key)
        {
            var fkey = FKeyToInt(key);
            return Values.FirstOrDefault(x => x.FKey == fkey);
        }

        public void RemovePerformance(Keys key)
        {
            RemovePerformance(GetPerformance(key)?.Title);
        }

        public void RemovePerformance(string title)
        {
            if (title == null)
                return;

            if (Remove(title))
                FireChanged();
        }

        public void SavePerformance(Keys key, string title, string data)
        {
            ClearPerformanceKey(key);

            title = title?.Trim();
            if (string.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title), "Title may not be empty.");

            this[title] = new ConfigPerformance
            {
                FKey = FKeyToInt(key),
                Title = title,
                Data = data
            };

            FireChanged();
        }
    }
}
