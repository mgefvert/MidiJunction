using System;

namespace MidiJunction.Devices
{
  public interface IMidiOutput : IDisposable
  {
    string Name { get; }
    void SendAllNotesOff();
    void SendAllNotesOff(int channel);
    void SendCommand(byte[] command);
    void SendCommand(MidiMessage message);
  }
}
