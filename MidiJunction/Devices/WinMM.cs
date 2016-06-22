using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MidiJunction.Devices
{
  // ReSharper disable InconsistentNaming

  public class WinMM
  {
    public const int MIM_OPEN = 961;
    public const int MIM_CLOSE = 962;
    public const int MIM_DATA = 963;
    public const int MIM_LONGDATA = 964;
    public const int MIM_ERROR = 965;
    public const int MIM_LONGERROR = 966;
    public const int MIM_MOREDATA = 972;

    [StructLayout(LayoutKind.Sequential)]
    public struct MIDIHDR
    {
      public IntPtr data;
      public uint bufferLength;
      public uint bytesRecorded;
      public IntPtr user;
      public uint flags;
      public IntPtr next;
      public IntPtr reserved;
      public uint offset;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public IntPtr[] reservedArray;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MIDIINCAPS
    {
      public ushort wMid;
      public ushort wPid;
      public uint vDriverVersion;     // MMVERSION
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
      public string szPname;
      public uint dwSupport;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MIDIOUTCAPS
    {
      public ushort wMid;
      public ushort wPid;
      public uint vDriverVersion;     // MMVERSION
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
      public string szPname;
      public MIDIOUTCAPS_TECHNOLOGY wTechnology;
      public ushort wVoices;
      public ushort wNotes;
      public ushort wChannelMask;
      public MIDIOUTCAPS_SUPPORT dwSupport;
    }

    public enum MIDIOUTCAPS_TECHNOLOGY : ushort
    {
      MOD_MIDIPORT = 1,     // output port
      MOD_SYNTH = 2,        // generic internal synth
      MOD_SQSYNTH = 3,      // square wave internal synth
      MOD_FMSYNTH = 4,      // FM internal synth
      MOD_MAPPER = 5,       // MIDI mapper
      MOD_WAVETABLE = 6,    // hardware wavetable synth
      MOD_SWSYNTH = 7       // software synth
    }

    public enum MIDIOUTCAPS_SUPPORT : uint
    {
      MIDICAPS_VOLUME = 1,      // supports volume control
      MIDICAPS_LRVOLUME = 2,    // separate left-right volume control
      MIDICAPS_CACHE = 4,
      MIDICAPS_STREAM = 8       // driver supports midiStreamOut directly
    }

    public enum MMRESULT : uint
    {
      MMSYSERR_NOERROR = 0,
      MMSYSERR_ERROR = 1,
      MMSYSERR_BADDEVICEID = 2,
      MMSYSERR_NOTENABLED = 3,
      MMSYSERR_ALLOCATED = 4,
      MMSYSERR_INVALHANDLE = 5,
      MMSYSERR_NODRIVER = 6,
      MMSYSERR_NOMEM = 7,
      MMSYSERR_NOTSUPPORTED = 8,
      MMSYSERR_BADERRNUM = 9,
      MMSYSERR_INVALFLAG = 10,
      MMSYSERR_INVALPARAM = 11,
      MMSYSERR_HANDLEBUSY = 12,
      MMSYSERR_INVALIDALIAS = 13,
      MMSYSERR_BADDB = 14,
      MMSYSERR_KEYNOTFOUND = 15,
      MMSYSERR_READERROR = 16,
      MMSYSERR_WRITEERROR = 17,
      MMSYSERR_DELETEERROR = 18,
      MMSYSERR_VALNOTFOUND = 19,
      MMSYSERR_NODRIVERCB = 20,
      WAVERR_BADFORMAT = 32,
      WAVERR_STILLPLAYING = 33,
      WAVERR_UNPREPARED = 34
    }

    public const int CALLBACK_TYPEMASK = 0x00070000; /* callback type mask */
    public const int CALLBACK_NULL = 0x00000000; /* no callback */
    public const int CALLBACK_WINDOW = 0x00010000; /* dwCallback is a HWND */
    public const int CALLBACK_TASK = 0x00020000; /* dwCallback is a HTASK */
    public const int CALLBACK_FUNCTION = 0x00030000; /* dwCallback is a FARPROC */

    public delegate void MidiCallback(int handle, int msg, int instance, int param1, int param2);

    [DllImport("winmm.dll")]
    public static extern MMRESULT midiConnect(IntPtr hMidi, IntPtr hmo, IntPtr pReserved);
    [DllImport("winmm.dll")]
    public static extern MMRESULT midiDisconnect(IntPtr hMidi, IntPtr hmo, IntPtr pReserved);
    [DllImport("winmm.dll", SetLastError = true)]
    public static extern MMRESULT midiInClose(IntPtr hMidiIn);
    [DllImport("winmm.dll", SetLastError = true)]
    public static extern MMRESULT midiInGetDevCaps(UIntPtr uDeviceID, ref MIDIINCAPS caps, uint cbMidiInCaps);
    [DllImport("winmm.dll", SetLastError = true)]
    public static extern uint midiInGetNumDevs();
    [DllImport("winmm.dll", SetLastError = true)]
    public static extern MMRESULT midiInOpen(out IntPtr lphMidiIn, uint uDeviceID, MidiCallback dwCallback, IntPtr dwInstance, uint dwFlags);
    [DllImport("winmm.dll", SetLastError = true)]
    public static extern MMRESULT midiInReset(IntPtr hMidiIn);
    [DllImport("winmm.dll", SetLastError = true)]
    public static extern MMRESULT midiInStart(IntPtr hMidiIn);
    [DllImport("winmm.dll")]
    public static extern MMRESULT midiOutClose(IntPtr hMidiOut);
    [DllImport("winmm.dll", SetLastError = true)]
    public static extern MMRESULT midiOutGetDevCaps(UIntPtr uDeviceID, ref MIDIOUTCAPS lpMidiOutCaps, uint cbMidiOutCaps);
    [DllImport("winmm.dll")]
    public static extern MMRESULT midiOutGetErrorText(uint mmrError, StringBuilder pszText, uint cchText);
    [DllImport("winmm.dll", SetLastError = true)]
    public static extern uint midiOutGetNumDevs();
    [DllImport("winmm.dll")]
    public static extern MMRESULT midiOutOpen(out IntPtr lphMidiOut, uint uDeviceID, MidiCallback dwCallback, IntPtr dwInstance, uint dwFlags);
    [DllImport("winmm.dll")]
    public static extern MMRESULT midiOutShortMsg(IntPtr hMidiOut, uint dwMsg);
    [DllImport("winmm.dll")]
    public extern static MMRESULT midiStreamClose(IntPtr hMidiStream);
    [DllImport("winmm.dll")]
    public extern static MMRESULT midiStreamOpen(ref IntPtr hMidiStream, ref int puDeviceID, int cMidi, IntPtr dwCallback, IntPtr dwInstance, int fdwOpen);
    [DllImport("winmm.dll")]
    public static extern MMRESULT midiStreamOut(IntPtr hMidiStream, MIDIHDR lpMidiHdr, uint cbMidiHdr);
    [DllImport("winmm.dll")]
    public static extern MMRESULT midiStreamPause(IntPtr hMidiStream);
    [DllImport("winmm.dll")]
    public static extern MMRESULT midiStreamRestart(IntPtr hMidiStream);
    [DllImport("winmm.dll")]
    public static extern MMRESULT midiStreamStop(IntPtr hMidiStream);
  }
}
