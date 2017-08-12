using System;
using System.Threading;
using DotNetCommons;
using NAudio.CoreAudioApi;

namespace MidiJunction.Devices
{
    public class SystemVolume
    {
        private readonly MMDeviceEnumerator _deviceEnumerator = new MMDeviceEnumerator();
        private readonly MMDevice _playbackDevice;
        private readonly Timer _timer;

        public bool Mute => _playbackDevice.AudioEndpointVolume.Mute;
        public int? TargetVolume { get; set; }

        public int CurrentVolume
        {
            get
            {
                try
                {
                    return (int)Math.Round(_playbackDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
                }
                catch
                {
                    return 0;
                }
            }
            set => _playbackDevice.AudioEndpointVolume.MasterVolumeLevelScalar = (value / 100.0f + 0.00001f).Limit(0, 1);
        }

        public SystemVolume()
        {
            _timer = new Timer(TimerCallback, null, TimeSpan.FromMilliseconds(50), TimeSpan.FromMilliseconds(50));

            try
            {
                _playbackDevice = _deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            }
            catch
            {
                //
            }
        }

        private void TimerCallback(object state)
        {
            if (TargetVolume == null)
                return;

            var current = CurrentVolume;

            var diff = Math.Abs(TargetVolume.Value - current);
            if (diff == 0)
            {
                TargetVolume = null;
                return;
            }

            var change = (diff / 4).Limit(1, 10);
            CurrentVolume = TargetVolume.Value < current ? current - change : current + change;
        }

        public void Increase10()
        {
            var v = (TargetVolume ?? CurrentVolume) + 2;
            TargetVolume = Math.Min((v / 10 + 1) * 10, 100);
        }

        public void Decrease10()
        {
            var v = (TargetVolume ?? CurrentVolume) + 2;
            TargetVolume = Math.Max((v / 10 - 1) * 10, 0);
        }
    }
}
