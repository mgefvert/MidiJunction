using System;
using NAudio.CoreAudioApi;

namespace MidiJunction.Devices
{
  public class SystemVolume
  {
    private readonly MMDeviceEnumerator _deviceEnumerator = new MMDeviceEnumerator();
    private readonly MMDevice _playbackDevice;

    public SystemVolume()
    {
      try
      {
        _playbackDevice = _deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
      }
      catch
      {
        //
      }
    }

    public int GetVolume()
    {
      try
      {
        return (int)(_playbackDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
      }
      catch
      {
        return 0;
      }
    }

    public void SetVolume(int volume)
    {
      var v = volume / 100.0 + 0.00001;

      if (v < 0.0)
        v = 0.0;
      if (v > 1.0)
        v = 1.0;

      _playbackDevice.AudioEndpointVolume.MasterVolumeLevelScalar = (float)v;
    }

    public void Increase10()
    {
      var v = GetVolume() + 2;
      SetVolume((v / 10 + 1) * 10);
    }

    public void Decrease10()
    {
      var v = GetVolume() + 2;
      SetVolume((v / 10 - 1) * 10);
    }
  }
}
