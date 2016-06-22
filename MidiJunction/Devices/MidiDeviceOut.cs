using System;

namespace MidiJunction.Devices
{
  public class MidiDeviceOut : MidiDevice
  {
    private readonly IntPtr _handle;

    internal MidiDeviceOut(int device) : base(device, Midi.OutDeviceInfo(device).szPname)
    {
      CheckResult(WinMM.midiOutOpen(out _handle, (uint)device, MidiHandler, (IntPtr)0, WinMM.CALLBACK_FUNCTION));
    }

    public override void Dispose()
    {
      WinMM.midiOutClose(_handle);
    }

    public void Send(MidiMessage msg)
    {
      CheckResult(WinMM.midiOutShortMsg(_handle, msg.Value));
    }
  }
}
