using System;
using System.Threading;
using NAudio.CoreAudioApi;

namespace MidiJunction.Devices
{
    public class SystemVolume
    {
        private readonly MMDeviceEnumerator _deviceEnumerator = new MMDeviceEnumerator();
        private readonly MMDevice _playbackDevice;
        private readonly Timer _timer;

        public float? TargetVolume { get; set; }

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

            var c = GetVolume();
            var diff = TargetVolume.Value - c;

            var change = Math.Min(2, Math.Abs(diff));
            if (change < 0.1)
            {
                TargetVolume = null;
                return;
            }

            if (c > TargetVolume)
                change = -change;

            SetVolume(c + change);
        }

        public float GetVolume()
        {
            try
            {
                return _playbackDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100;
            }
            catch
            {
                return 0;
            }
        }

        public void SetVolume(float volume)
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
            var v = (int)(TargetVolume ?? GetVolume()) + 2;
            TargetVolume = Math.Min((v / 10 + 1) * 10, 100);
        }

        public void Decrease10()
        {
            var v = (int)(TargetVolume ?? GetVolume()) + 2;
            TargetVolume = Math.Max((v / 10 - 1) * 10, 0);
        }
    }
}
