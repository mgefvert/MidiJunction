/* teVirtualMIDI C# interface - v1.2.9.37
 *
 * Copyright 2009-2015, Tobias Erichsen
 * All rights reserved, unauthorized usage & distribution is prohibited.
 *
 * For technical or commercial requests contact: info <at> tobias-erichsen <dot> de
 *
 * teVirtualMIDI.sys is a kernel-mode device-driver which can be used to dynamically create & destroy
 * midiports on Windows (XP to Windows 7, 32bit & 64bit).  The "back-end" of teVirtualMIDI can be used
 * to create & destroy such ports and receive and transmit data from/to those created ports.
 *
 * File: TeVirtualMIDI.cs
 *
 * This file implements the C#-class-wrapper for the teVirtualMIDI-driver.
 * This class encapsualtes the native C-type interface which is integrated
 * in the teVirtualMIDI32.dll and the teVirtualMIDI64.dll.
 */

using System;
using System.Runtime.InteropServices;

namespace MidiJunction.Devices
{
  [Serializable]
  public class VirtualMidiException : Exception
  {
    // ReSharper disable InconsistentNaming

    /* defines of specific WIN32-error-codes that the native teVirtualMIDI-driver
     * is using to communicate specific problems to the application */
    private const int ERROR_PATH_NOT_FOUND = 3;
    private const int ERROR_INVALID_HANDLE = 6;
    private const int ERROR_TOO_MANY_CMDS = 56;
    private const int ERROR_TOO_MANY_SESS = 69;
    private const int ERROR_INVALID_NAME = 123;
    private const int ERROR_MOD_NOT_FOUND = 126;
    private const int ERROR_BAD_ARGUMENTS = 160;
    private const int ERROR_ALREADY_EXISTS = 183;
    private const int ERROR_OLD_WIN_VERSION = 1150;
    private const int ERROR_REVISION_MISMATCH = 1306;
    private const int ERROR_ALIAS_EXISTS = 1379;

    // ReSharper restore InconsistentNaming

    public int ReasonCode { get; }

    public VirtualMidiException(int code) : base(ReasonCodeToString(code))
    {
      ReasonCode = code;
    }

    private static string ReasonCodeToString(int reasonCode)
    {
      switch (reasonCode)
      {
        case ERROR_OLD_WIN_VERSION:
          return "Your Windows-version is too old for dynamic MIDI-port creation.";
        case ERROR_INVALID_NAME:
          return "You need to specify at least 1 character as MIDI-portname!";
        case ERROR_ALREADY_EXISTS:
          return "The name for the MIDI-port you specified is already in use!";
        case ERROR_ALIAS_EXISTS:
          return "The name for the MIDI-port you specified is already in use!";
        case ERROR_PATH_NOT_FOUND:
          return "Possibly the teVirtualMIDI-driver has not been installed!";
        case ERROR_MOD_NOT_FOUND:
          return "The teVirtualMIDIxx.dll could not be loaded!";
        case ERROR_REVISION_MISMATCH:
          return "The teVirtualMIDIxx.dll and teVirtualMIDI.sys driver differ in version!";
        case ERROR_TOO_MANY_SESS:
          return "Maximum number of ports reached";
        case ERROR_INVALID_HANDLE:
          return "Port not enabled";
        case ERROR_TOO_MANY_CMDS:
          return "MIDI-command too large";
        case ERROR_BAD_ARGUMENTS:
          return "Invalid flags specified";
        default:
          return "Unspecified virtualMIDI-error: " + reasonCode;
      }
    }
  }

  public class VirtualMidi : IMidiOutput
  {
    // ReSharper disable InconsistentNaming

    /* default size of sysex-buffer */
    private const uint TE_VM_DEFAULT_SYSEX_SIZE = 65535;
    /* constant for loading of teVirtualMIDI-interface-DLL, either 32 or 64 bit */
    private const string DllName = "teVirtualMIDI.dll";
    /* private const string DllName = "teVirtualMIDI32.dll"; */
    /* private const string DllName = "teVirtualMIDI64.dll"; */

    /* TE_VM_LOGGING_MISC - log internal stuff (port enable, disable...) */
    public const uint TE_VM_LOGGING_MISC = 1;
    /* TE_VM_LOGGING_RX - log data received from the driver */
    public const uint TE_VM_LOGGING_RX = 2;
    /* TE_VM_LOGGING_TX - log data sent to the driver */
    public const uint TE_VM_LOGGING_TX = 4;

    /* TE_VM_FLAGS_PARSE_RX - parse incoming data into single, valid MIDI-commands */
    public const uint TE_VM_FLAGS_PARSE_RX = 1;
    /* TE_VM_FLAGS_PARSE_TX - parse outgoing data into single, valid MIDI-commands */
    public const uint TE_VM_FLAGS_PARSE_TX = 2;
    /* TE_VM_FLAGS_INSTANTIATE_RX_ONLY - Only the "midi-out" part of the port is created */
    public const uint TE_VM_FLAGS_INSTANTIATE_RX_ONLY = 4;
    /* TE_VM_FLAGS_INSTANTIATE_TX_ONLY - Only the "midi-in" part of the port is created */
    public const uint TE_VM_FLAGS_INSTANTIATE_TX_ONLY = 8;
    /* TE_VM_FLAGS_INSTANTIATE_BOTH - a bidirectional port is created */
    public const uint TE_VM_FLAGS_INSTANTIATE_BOTH = 12;

    // ReSharper restore InconsistentNaming

    private readonly byte[] _readBuffer;
    private IntPtr _instance;
    private readonly uint _maxSysexLength;
    private readonly ulong[] _readProcessIds;

    public static string VersionString { get; }
    public static string DriverVersionString { get; }

    public string Name { get; }

    /* static initializer to retrieve version-info from DLL... */
    static VirtualMidi()
    {
      ushort dummy = 0;

      VersionString = Marshal.PtrToStringAuto(virtualMIDIGetVersion(ref dummy, ref dummy, ref dummy, ref dummy));
      DriverVersionString = Marshal.PtrToStringAuto(virtualMIDIGetDriverVersion(ref dummy, ref dummy, ref dummy, ref dummy));
    }

    public VirtualMidi(string portName, uint maxSysexLength = TE_VM_DEFAULT_SYSEX_SIZE, uint flags = TE_VM_FLAGS_PARSE_RX)
    {
      _instance = virtualMIDICreatePortEx2(portName, IntPtr.Zero, IntPtr.Zero, maxSysexLength, flags);
      if (_instance == IntPtr.Zero)
        throw new VirtualMidiException(Marshal.GetLastWin32Error());

      _readBuffer = new byte[maxSysexLength];
      _readProcessIds = new ulong[17];
      _maxSysexLength = maxSysexLength;
      Name = portName;
    }

    public VirtualMidi(string portName, uint maxSysexLength, uint flags, ref Guid manufacturer, ref Guid product)
    {
      _instance = virtualMIDICreatePortEx3(portName, IntPtr.Zero, IntPtr.Zero, maxSysexLength, flags, ref manufacturer, ref product);
      if (_instance == IntPtr.Zero)
        throw new VirtualMidiException(Marshal.GetLastWin32Error());

      _readBuffer = new byte[maxSysexLength];
      _readProcessIds = new ulong[17];
      _maxSysexLength = maxSysexLength;
      Name = portName;
    }

    ~VirtualMidi()
    {
      Dispose();
    }

    public void Dispose()
    {
      if (_instance == IntPtr.Zero)
        return;

      virtualMIDIClosePort(_instance);
      _instance = IntPtr.Zero;
    }

    public static uint Logging(uint loggingMask)
    {
      return virtualMIDILogging(loggingMask);
    }

    public void Shutdown()
    {
      if (!virtualMIDIShutdown(_instance))
        throw new VirtualMidiException(Marshal.GetLastWin32Error());
    }

    public void SendAllNotesOff()
    {
      for (var channel = 0; channel < 16; channel++)
        SendAllNotesOff(channel);
    }

    public void SendAllNotesOff(int channel)
    {
      for (var note = 0; note <= 0x7F; note++)
        SendCommand(MidiMessage.NoteOff(channel, note, 0).Bytes);

      SendCommand(MidiMessage.ControlChange(channel, (int)MidiControlMessage.Sustain, 0));
      SendCommand(MidiMessage.ControlChange(channel, (int)MidiControlMessage.AllSoundOff, 0));
      SendCommand(MidiMessage.ControlChange(channel, (int)MidiControlMessage.AllNotesOff, 0));
    }

    public void SendCommand(byte[] command)
    {
      if (command == null || command.Length == 0)
        return;

      if (!virtualMIDISendData(_instance, command, (uint)command.Length))
        throw new VirtualMidiException(Marshal.GetLastWin32Error());
    }

    public void SendCommand(MidiMessage message)
    {
      SendCommand(message.Bytes);
    }

    public byte[] GetCommand()
    {
      var length = _maxSysexLength;
      if (!virtualMIDIGetData(_instance, _readBuffer, ref length))
        throw new VirtualMidiException(Marshal.GetLastWin32Error());

      var outBytes = new byte[length];
      Array.Copy(_readBuffer, outBytes, length);

      return outBytes;
    }

    public ulong[] GetProcessIds()
    {
      uint length = 17 * sizeof(ulong);
      if (!virtualMIDIGetProcesses(_instance, _readProcessIds, ref length))
        throw new VirtualMidiException(Marshal.GetLastWin32Error());

      var count = length / sizeof(ulong);
      var outIds = new ulong[count];
      Array.Copy(_readProcessIds, outIds, count);

      return outIds;
    }

    [DllImport(DllName, EntryPoint = "virtualMIDICreatePortEx3", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr virtualMIDICreatePortEx3(string portName, IntPtr callback, IntPtr dwCallbackInstance, uint maxSysexLength, uint flags, ref Guid manufacturer, ref Guid product);

    [DllImport(DllName, EntryPoint = "virtualMIDICreatePortEx2", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr virtualMIDICreatePortEx2(string portName, IntPtr callback, IntPtr dwCallbackInstance, uint maxSysexLength, uint flags);

    [DllImport(DllName, EntryPoint = "virtualMIDIClosePort", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern void virtualMIDIClosePort(IntPtr instance);

    [DllImport(DllName, EntryPoint = "virtualMIDIShutdown", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern bool virtualMIDIShutdown(IntPtr instance);

    [DllImport(DllName, EntryPoint = "virtualMIDISendData", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern bool virtualMIDISendData(IntPtr midiPort, byte[] midiDataBytes, uint length);

    [DllImport(DllName, EntryPoint = "virtualMIDIGetData", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern bool virtualMIDIGetData(IntPtr midiPort, [Out] byte[] midiDataBytes, ref uint length);

    [DllImport(DllName, EntryPoint = "virtualMIDIGetProcesses", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern bool virtualMIDIGetProcesses(IntPtr midiPort, [Out] ulong[] processIds, ref uint length);

    [DllImport(DllName, EntryPoint = "virtualMIDIGetVersion", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr virtualMIDIGetVersion(ref ushort major, ref ushort minor, ref ushort release, ref ushort build);

    [DllImport(DllName, EntryPoint = "virtualMIDIGetDriverVersion", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr virtualMIDIGetDriverVersion(ref ushort major, ref ushort minor, ref ushort release, ref ushort build);

    [DllImport(DllName, EntryPoint = "virtualMIDILogging", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern uint virtualMIDILogging(uint loggingMask);
  }
}
