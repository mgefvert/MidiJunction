using System;

namespace MidiJunction.Devices
{
  public static class MidiDeviceManager
  {
    private static readonly object Lock = new object();
    private static volatile IMidiInput _inputDevice;
    private static volatile IMidiOutput _outputDevice;

    public static bool ConnectedToInput => _inputDevice is MidiDeviceIn;
    public static IMidiInput InputDevice => _inputDevice;
    public static IMidiOutput OutputDevice => _outputDevice;

    public static void Shutdown()
    {
      lock (Lock)
      {
        _inputDevice?.Dispose();
        _inputDevice = null;

        _outputDevice?.Dispose();
        _outputDevice = null;
      }
    }

    public static void CreateOutputDevice(string name)
    {
      lock (Lock)
      {
        _outputDevice?.Dispose();

        _outputDevice = string.IsNullOrEmpty(name)
            ? new NullOutputDevice()
            : (IMidiOutput)new VirtualMidi(name);
      }
    }

    public static void RescanInputDevice(string name, MidiMessageHandler handler)
    {
      lock (Lock)
      {
        var id = _inputDevice is MidiDeviceIn ? _inputDevice.Id : -1;
        var found = Midi.InFindDevice(name);

        // No MIDI input device found
        if (found == -1)
        {
          if (id != -1 || _inputDevice == null)
          {
            _inputDevice?.Dispose();
            _inputDevice = new NullInputDevice(name);
          }

          return;
        }

        // Device found, determine if we have the right one opened
        if (id == found)
          return;

        // 
        _inputDevice?.Dispose();
        _inputDevice = new MidiDeviceIn(found);
        _inputDevice.Message += handler;
      }
    }
  }
}
