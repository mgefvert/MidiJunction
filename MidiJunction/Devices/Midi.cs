using System;

namespace MidiJunction.Devices
{
  public static class Midi
  {
    public static int InDeviceCount => (int)WinMM.midiInGetNumDevs();
    public static int OutDeviceCount => (int)WinMM.midiOutGetNumDevs();

    public static WinMM.MIDIINCAPS InDeviceInfo(int device)
    {
      var result = new WinMM.MIDIINCAPS();
      if (device != -1)
        WinMM.midiInGetDevCaps((UIntPtr)device, ref result, 44);
      return result;
    }

    public static WinMM.MIDIOUTCAPS OutDeviceInfo(int device)
    {
      var result = new WinMM.MIDIOUTCAPS();
      if (device != -1)
        WinMM.midiOutGetDevCaps((UIntPtr)device, ref result, 52);
      return result;
    }

    public static MidiDeviceIn InOpen(int device)
    {
      return new MidiDeviceIn(device);
    }

    public static MidiDeviceOut OutOpen(int device)
    {
      return new MidiDeviceOut(device);
    }

    public static int InFindDevice(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
        return -1;

      name = name.ToLower();
      for (var i = 0; i < InDeviceCount; i++)
        if (InDeviceInfo(i).szPname.ToLower().Contains(name))
          return i;

      return -1;
    }

    public static int OutFindDevice(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
        return -1;

      name = name.ToLower();
      for (var i = 0; i < OutDeviceCount; i++)
        if (OutDeviceInfo(i).szPname.ToLower().Contains(name))
          return i;

      return -1;
    }
  }
}
