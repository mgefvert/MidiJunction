using System;
using System.ComponentModel;

namespace MidiJunction.Devices
{
    public class MidiMessageEventArgs
    {
        public int Handle { get; }
        public MidiMessage Message { get; }

        public MidiMessageEventArgs(int handle, MidiMessage message)
        {
            Handle = handle;
            Message = message;
        }
    }

    public delegate void MidiMessageHandler(object sender, MidiMessageEventArgs args);

    public abstract class MidiDevice : IDisposable
    {
        public int Id { get; }
        public string Name { get; }
        public event MidiMessageHandler Message;
        protected bool Closing;

        protected MidiDevice(int device, string name)
        {
            Id = device;
            Name = name;
        }

        public abstract void Dispose();

        protected void CheckResult(WinMM.MMRESULT resultCode)
        {
            if (resultCode != WinMM.MMRESULT.MMSYSERR_NOERROR)
                throw new Win32Exception((int)resultCode);
        }

        protected void MidiHandler(int handle, int msg, int instance, int param1, int param2)
        {
            if (!Closing)
                Message?.Invoke(this, new MidiMessageEventArgs(handle, new MidiMessage((uint)param1)));
        }
    }
}
