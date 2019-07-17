using System;
using MidiJunction.Classes;

namespace MidiJunction.Devices
{
    public class MidiDeviceIn : MidiDevice
    {
        private readonly IntPtr _handle;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly WinMM.MidiCallback _callback;

        internal MidiDeviceIn(int device) : base(device, MidiDeviceManager.InDeviceInfo(device).szPname)
        {
            _callback = MidiHandler;

            CheckResult(WinMM.midiInOpen(out _handle, (uint)device, _callback, (IntPtr)0, WinMM.CALLBACK_FUNCTION));
            WinMM.midiInStart(_handle);
        }

        public override void Dispose()
        {
            Closing = true;

            WinMM.midiInReset(_handle);

            // Give the device manager a chance to process current messages ... it seems like only
            // one DoEvents() is necessary, but I don't trust it.
            Helper.WaitALittle(TimeSpan.FromMilliseconds(100));

            WinMM.midiInClose(_handle);
        }
    }
}
