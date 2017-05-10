using System;
using System.Drawing;
using System.Windows.Forms;
using MidiJunction.Configuration;

namespace MidiJunction.Classes
{
    public class ButtonInfo
    {
        private static readonly Color DarkGray = Color.FromArgb(51, 51, 51);
        private static readonly Color StandbyColor = Helper.Mix(Color.DodgerBlue, DarkGray, 50);

        private readonly Bitmap _bkgStandby1 = Helper.MakeBackground(StandbyColor);
        private readonly Bitmap _bkgStandby2 = Helper.MakeBackground(StandbyColor, 12);
        private readonly Bitmap _bkgStandby3 = Helper.MakeBackground(StandbyColor, 6);

        public ConfigButton Config { get; set; }
        public Button Button { get; set; }
        public Keys Key => Config.Key;
        public bool Active { get; set; }
        public int HotStandby { get; set; }

        public void Click()
        {
            Button.PerformClick();
        }

        public void Recolor()
        {
            Button.ForeColor = Active ? Color.Black : Color.White;

            if (Active)
            {
                Button.BackgroundImage = null;
                Button.BackColor = Color.Chartreuse;
                return;
            }

            Button.BackColor = Color.Transparent;
            if (HotStandby == 0)
            {
                Button.BackgroundImage = null;
                return;
            }

            switch (HotStandby)
            {
                case 2:
                    Button.BackgroundImage = _bkgStandby2;
                    break;
                case 3:
                    Button.BackgroundImage = _bkgStandby3;
                    break;
                default:
                    Button.BackgroundImage = _bkgStandby1;
                    break;
            }
        }
    }
}
