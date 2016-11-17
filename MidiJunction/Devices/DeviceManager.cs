using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace MidiJunction.Devices
{
    public static class MidiDeviceManager
    {
        private static readonly object Lock = new object();

        // ReSharper disable once InconsistentNaming
        private static readonly List<VirtualMidi> _outputDevices = new List<VirtualMidi>();
        private static volatile MidiDeviceIn _inputDevice;

        public static bool ConnectedToInput => _inputDevice != null;
        public static MidiDeviceIn InputDevice => _inputDevice;
        public static IReadOnlyCollection<VirtualMidi> OutputDevices => _outputDevices;

        public static int InDeviceCount => (int)WinMM.midiInGetNumDevs();

        public static WinMM.MIDIINCAPS InDeviceInfo(int device)
        {
            var result = new WinMM.MIDIINCAPS();
            if (device != -1)
                WinMM.midiInGetDevCaps((UIntPtr)device, ref result, 44);
            return result;
        }

        public static MidiDeviceIn OpenInDevice(int device)
        {
            return new MidiDeviceIn(device);
        }

        public static int FindInDevice(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return -1;

            name = name.ToLower();
            for (var i = 0; i < InDeviceCount; i++)
                if (InDeviceInfo(i).szPname.ToLower().Contains(name))
                    return i;

            return -1;
        }

        public static void ClearOutputDevices()
        {
            lock (Lock)
            {
                foreach (var device in _outputDevices)
                    device.Dispose();

                _outputDevices.Clear();
            }
        }

        public static void RescanInputDevice(string name, MidiMessageHandler handler)
        {
            lock (Lock)
            {
                var id = _inputDevice?.Id ?? -1;
                var found = FindInDevice(name);

                // No MIDI input device found
                if (found == -1)
                {
                    if (id != -1 || _inputDevice == null)
                    {
                        _inputDevice?.Dispose();
                        _inputDevice = null;
                    }

                    return;
                }

                // Device found, determine if we have the right one opened
                if (id == found)
                    return;

                _inputDevice?.Dispose();
                _inputDevice = new MidiDeviceIn(found);
                _inputDevice.Message += handler;
            }
        }

        public static void SetOutputDevices(List<string> names)
        {
            names = names.Distinct().ToList();

            if (_outputDevices.Count > 0)
            {
                ClearOutputDevices();

                // Give the driver a chance to clean up
                for (int i = 0; i < 50; i++)
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                }
            }

            lock (Lock)
            {
                foreach (var name in names)
                    _outputDevices.Add(new VirtualMidi(name));
            }
        }

        public static void SendAllNotesOff()
        {
            lock (Lock)
            {
                foreach (var device in _outputDevices)
                    device.SendAllNotesOff();
            }
        }

        public static void SendAllNotesOff(int device)
        {
            lock (Lock)
            {
                _outputDevices.ElementAtOrDefault(device)?.SendAllNotesOff();
            }
        }

        public static void SendAllNotesOff(int device, int channel)
        {
            lock (Lock)
            {
                _outputDevices.ElementAtOrDefault(device)?.SendAllNotesOff(channel);
            }
        }

        public static void Shutdown()
        {
            lock (Lock)
            {
                _inputDevice?.Dispose();
                _inputDevice = null;

                ClearOutputDevices();
            }
        }
    }
}
