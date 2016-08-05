using System;

namespace MidiJunction.Devices
{
  public class MidiDeviceIn : MidiDevice
  {
    private readonly IntPtr _handle;
    private readonly WinMM.MidiCallback _callback;

    internal MidiDeviceIn(int device) : base(device, MidiDeviceManager.InDeviceInfo(device).szPname)
    {
      _callback = MidiHandler;

      CheckResult(WinMM.midiInOpen(out _handle, (uint)device, _callback, (IntPtr)0, WinMM.CALLBACK_FUNCTION));
      WinMM.midiInStart(_handle);
    }

    public override void Dispose()
    {
      Closing = true;
      WinMM.midiInStop(_handle);
      WinMM.midiInClose(_handle);
    }
  }
}
