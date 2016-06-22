using System;

namespace MidiJunction.Devices
{
  public class NullOutputDevice : IMidiOutput
  {
    public string Name { get; } = "<None>";

    public void Dispose()
    {
    }

    public void SendAllNotesOff()
    {
    }

    public void SendAllNotesOff(int channel)
    {
    }

    public void SendCommand(byte[] command)
    {
    }

    public void SendCommand(MidiMessage message)
    {
    }
  }
}
