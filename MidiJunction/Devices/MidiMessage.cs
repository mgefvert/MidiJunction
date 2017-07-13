using System;
using System.Linq;

namespace MidiJunction.Devices
{
    public enum MidiControlMessage
    {
        BankSelectMSB = 0,
        ModulationWheelMSB = 1,
        BreathControllerMSB = 2,
        FootControllerMSB = 4,
        PortamentoTimeMSB = 5,
        DataEntryMSB = 6,
        ChannelVolumeMSB = 7,
        BalanceMSB = 8,
        PanMSB = 10,
        ExpressionControllerMSB = 11,
        EffectControl1MSB = 12,
        EffectControl2MSB = 13,
        GeneralPurposeController1MSB = 16,
        GeneralPurposeController2MSB = 17,
        GeneralPurposeController3MSB = 18,
        GeneralPurposeController4MSB = 19,
        BankSelectLSB = 32,
        ModulationWheelLSB = 33,
        BreathControllerLSB = 34,
        FootControllerLSB = 36,
        PortamentoTimeLSB = 37,
        DataEntryLSB = 38,
        ChannelVolumeLSB = 39,
        BalanceLSB = 40,
        PanLSB = 42,
        ExpressionControllerLSB = 43,
        EffectControl1LSB = 44,
        EffectControl2LSB = 45,
        GeneralPurposeController1LSB = 48,
        GeneralPurposeController2LSB = 49,
        GeneralPurposeController3LSB = 50,
        GeneralPurposeController4LSB = 51,
        Sustain = 64,
        Portamento = 65,
        Sostenuto = 66,
        SoftPedal = 67,
        Legato = 68,
        Hold2 = 69,
        SoundController1 = 70,
        SoundController2 = 71,
        SoundController3 = 72,
        SoundController4 = 73,
        SoundController5 = 74,
        SoundController6 = 75,
        SoundController7 = 76,
        SoundController8 = 77,
        SoundController9 = 78,
        SoundController10 = 79,
        GeneralPurposeController5 = 80,
        GeneralPurposeController6 = 81,
        GeneralPurposeController7 = 82,
        GeneralPurposeController8 = 83,
        PortamentoControl = 84,
        HiResVelocityPrefix = 88,
        Effects1Depth = 91,
        Effects2Depth = 92,
        Effects3Depth = 93,
        Effects4Depth = 94,
        Effects5Depth = 95,
        DataIncrement = 96,
        DataDecrement = 97,
        NonRegisteredParameterNumberLSB = 98,
        NonRegisteredParameterNumberMSB = 99,
        RegisteredParameterNumberLSB = 100,
        RegisteredParameterNumberMSB = 101,
        AllSoundOff = 120,
        ResetAllControllers = 121,
        LocalControl = 122,
        AllNotesOff = 123,
        OmniModeOff = 124,
        OmniModeOn = 125,
        MonoModeOn = 126,
        PolyModeOn = 127
    }

    public class MidiMessage
    {
        public uint Value { get; set; }
        public DateTime Time { get; set; }

        public bool IsControlMessage => (Status & 0xF0u) == 0xB0u && Data1 < 120;
        public bool IsChannelMessage => (Status & 0xF0u) != 0xF0u;
        public bool IsSystemMessage => (Status & 0xF0u) == 0xF0u;
        public bool IsValid => Status >= 0x80u && Data1 < 0x80u && Data2 < 0x80u;

        public byte[] Bytes => new[] { Status, Data1, Data2 };

        public bool IsNoteOffMessage => (Status & 0xF0u) == 0x80u || (Status & 0xF0u) == 0x90 && Data2 == 0;
        public bool IsNoteOnMessage => (Status & 0xF0u) == 0x90u && Data2 > 0;
        public bool IsNoteMessage => IsNoteOnMessage || IsNoteOffMessage;

        public MidiMessage()
        {
            Time = DateTime.Now;
        }

        public MidiMessage(uint msg) : this()
        {
            Value = msg;
        }

        public MidiMessage(byte command, byte data1, byte data2) : this()
        {
            Status = command;
            Data1 = data1;
            Data2 = data2;
        }

        // 1000nnnn = Note off
        public static MidiMessage NoteOff(int channel, int note, int velocity)
        {
            return new MidiMessage((byte)(0x80 + (channel & 0x0Fu)), (byte)note, (byte)velocity);
        }

        // 1001nnnn = Note on
        public static MidiMessage NoteOn(int channel, int note, int velocity)
        {
            return new MidiMessage((byte)(0x90 + (channel & 0x0Fu)), (byte)note, (byte)velocity);
        }

        // 1010nnnn = Polyphonic Key Pressure
        public static MidiMessage KeyPressure(int channel, int note, int pressure)
        {
            return new MidiMessage((byte)(0xA0 + (channel & 0x0Fu)), (byte)note, (byte)pressure);
        }

        // 1011nnnn = Control Change
        public static MidiMessage ControlChange(int channel, int controller, int value)
        {
            return new MidiMessage((byte)(0xB0 + (channel & 0x0Fu)), (byte)controller, (byte)value);
        }

        // 1100nnnn = Program Change
        public static MidiMessage ProgramChange(int channel, int program)
        {
            return new MidiMessage((byte)(0xC0 + (channel & 0x0Fu)), (byte)program, 0);
        }

        // 1101nnnn = Channel Pressure (Aftertouch)
        public static MidiMessage ChannelPressure(int channel, int pressure)
        {
            return new MidiMessage((byte)(0xD0 + (channel & 0x0Fu)), (byte)pressure, 0);
        }

        public byte Status
        {
            get { return (byte)(Value & 0xFFu); }
            set { Value = (Value & 0xFFFF00u) | value; }
        }

        public byte Channel
        {
            get { return (byte)(Value & 0x0Fu); }
            set { Value = ((Value & 0xFFFFF0u) | (value & 0x0Fu)); }
        }

        public byte Data1
        {
            get { return (byte)((Value >> 8) & 0xFFu); }
            set { Value = Value & 0xFF00FFu | ((uint)value << 8); }
        }

        public byte Data2
        {
            get { return (byte)((Value >> 16) & 0xFFu); }
            set { Value = Value & 0x00FFFFu | ((uint)value << 16); }
        }

        public override string ToString()
        {
            return string.Join(":", Bytes.Select(x => x.ToString("X2")));
        }
    }
}
