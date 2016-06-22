using System;

namespace MidiJunction.Devices
{
  public class NullInputDevice : IMidiInput
  {
    public int Id { get; }
    public string Name { get; }
    public event MidiMessageHandler Message;

    public NullInputDevice(string name)
    {
      Id = 0;
      Name = name;
    }

    public void Dispose()
    {
    }

    protected virtual void OnMessage(MidiMessageEventArgs args)
    {
      Message?.Invoke(this, args);
    }
  }
}
