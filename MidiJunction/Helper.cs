using System;

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
  }
}
