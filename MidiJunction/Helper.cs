using System;
using System.Drawing;

namespace MidiJunction
{
  public static class Helper
  {
    public static int Limit(int value, int min, int max)
    {
      if (value < min)
        return min;

      if (value > max)
        return max;

      return value;
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

    public static Rectangle ToRectangle(this RectangleF r)
    {
      return new Rectangle((int)r.X, (int)r.Y, (int)r.Width, (int)r.Height);
    }

    public static Color SetAlpha(this Color color, byte alpha)
    {
      return Color.FromArgb(alpha, color.R, color.G, color.B);
    }
  }
}
