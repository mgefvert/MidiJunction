using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;

namespace MidiJunction
{
    public static class Helper
    {
        public static string EnumToString<T>(T value, bool hideDefault) where T : struct
        {
            return value.Equals(default(T)) ? "" : value.ToString();
        }

        public static string KeyToString(Keys key)
        {
            return key >= Keys.D0 && key <= Keys.D9 ? key.ToString().Substring(1) : key.ToString();
        }

        public static int Limit(int value, int min, int max)
        {
            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }

        public static Bitmap MakeBackground(Color c)
        {
            var result = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

            using (var g = Graphics.FromImage(result))
            {
                g.Clear(c);
            }

            return result;
        }

        public static Bitmap MakeBackground(Color c, int width)
        {
            var result = new Bitmap(128, 128, PixelFormat.Format32bppArgb);

            using (var g = Graphics.FromImage(result))
            using (var brush = new SolidBrush(c))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                for (var x = 0; x < 256+width; x += 16)
                   g.FillPolygon(brush, new[] { new PointF(x, 0), new PointF(x+width, 0), new PointF(x-64+width, 128), new PointF(x-64, 128) });
            }

            return result;
        }

        public static Color Mix(Color highlightColor, Color baseColor, int percent)
        {
            var a = percent / 100.0;
            return Color.FromArgb(
                (int)(highlightColor.R * a + baseColor.R * (1 - a)),
                (int)(highlightColor.G * a + baseColor.G * (1 - a)),
                (int)(highlightColor.B * a + baseColor.B * (1 - a))
            );
        }

        public static Rectangle Modify(this Rectangle r, int x, int y, int width, int height)
        {
            return new Rectangle(r.X + x, r.Y + y, r.Width + width, r.Height + height);
        }

        public static RectangleF Modify(this RectangleF r, float x, float y, float width, float height)
        {
            return new RectangleF(r.X + x, r.Y + y, r.Width + width, r.Height + height);
        }

        public static Color SetAlpha(this Color color, byte alpha)
        {
            return Color.FromArgb(alpha, color.R, color.G, color.B);
        }

        public static Keys StringToKey(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Keys.None;

            if (value.Length == 1 && value[0] >= '0' && value[0] <= '9')
                value = "D" + value;

            Keys result;
            return Enum.TryParse(value, true, out result) ? result : Keys.None;
        }

        public static T StringToEnum<T>(string value) where T : struct
        {
            if (string.IsNullOrWhiteSpace(value))
                return default(T);

            T result;
            return Enum.TryParse(value, true, out result) ? result : default(T);
        }

        public static Rectangle ToRectangle(this RectangleF r)
        {
            return new Rectangle((int)r.X, (int)r.Y, (int)r.Width, (int)r.Height);
        }

        public static void WaitALittle(TimeSpan wait)
        {
            var target = DateTime.Now.Add(wait);
            while (DateTime.Now < target)
            {
                Application.DoEvents();
                Thread.Sleep(10);
            }
        }
    }
}
