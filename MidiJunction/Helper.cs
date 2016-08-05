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

    public static Color Mix(Color highlightColor, Color baseColor, int amount)
    {
      var a = amount / 100.0;
      return Color.FromArgb(
          (int)(highlightColor.R * a + baseColor.R * (1 - a)),
          (int)(highlightColor.G * a + baseColor.G * (1 - a)),
          (int)(highlightColor.B * a + baseColor.B * (1 - a))
      );
    }
  }
}
