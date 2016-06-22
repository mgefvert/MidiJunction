using System;

namespace MidiJunction.Devices
{
  public interface IMidiInput : IDisposable
  {
    int Id { get; }
    string Name { get; }
    event MidiMessageHandler Message;
  }
}
