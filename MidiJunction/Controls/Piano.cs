using System;
using System.Drawing;
using System.Windows.Forms;
using MidiJunction.Classes;

namespace MidiJunction.Controls
{
    public partial class Piano : UserControl
    {
        private static readonly float[] BlackOffset = { -0.1f, 0.1f, 0, -0.1f, 0, 0.1f, 0, 0 };
        private const int WhiteKeys = 52;
        private const int MiddleC = 23;

        private Rectangle _clientRect;
        private int _blackKeyHeight;
        private float _blackKeyWidth;
        private int _whiteKeyHeight;
        private float _whiteKeyWidth;

        private readonly byte[] _keyDown = new byte[128];

        public Piano()
        {
            InitializeComponent();
        }

        public void SetKeyDown(byte note, byte value)
        {
            if ((note & 0x80) != 0)
                return;

            _keyDown[note] = value > 127 ? (byte)127 : value;
            Invalidate();
        }

        public void ClearKey(byte note)
        {
            SetKeyDown(note, 0);
        }

        public void ClearAllKeys()
        {
            for (int i = 0; i < _keyDown.Length; i++)
                _keyDown[i] = 0;

            Invalidate();
        }

        protected Rectangle WhiteKeyRect(int key)
        {
            return new Rectangle(
              (int)(_clientRect.Left + _whiteKeyWidth * key),
              _clientRect.Top,
              (int)_whiteKeyWidth,
              _whiteKeyHeight
            );
        }

        // Black keys are regarded as flat keys (B -> Bb)
        protected Rectangle? BlackKeyRect(int key)
        {
            var n = (key + 4) % 7;
            if (key == 0 || (n != 0 && n != 1 && n != 3 && n != 4 && n != 5))
                return null;

            return new Rectangle(
              (int)(_clientRect.Left + _whiteKeyWidth * key - _blackKeyWidth / 2 + _whiteKeyWidth * BlackOffset[n]),
              _clientRect.Top,
              (int)_blackKeyWidth,
              _blackKeyHeight
            );
        }

        protected Rectangle Offset(Rectangle rect, int left, int top, int width, int height)
        {
            return new Rectangle(rect.Left + left, rect.Top + top, rect.Width + width, rect.Height + height);
        }

        protected int NoteIndex(int key, bool black)
        {
            if (key == 0)
                return 21;
            if (key == 1)
                return black ? 22 : 23;

            var octave = key < 2 ? -1 : (key - 2) / 7;
            var note = (key - 2) % 7;

            return octave * 12 + note * 2 - (note >= 3 ? 1 : 0) - (black ? 1 : 0) + 24;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            UpdateMeasurements();
            Invalidate();
        }

        private void UpdateMeasurements()
        {
            _clientRect = ClientRectangle;

            _whiteKeyHeight = _clientRect.Height;
            _whiteKeyWidth = (float)_clientRect.Width / WhiteKeys;
            _blackKeyWidth = _whiteKeyWidth * 0.7f;
            _blackKeyHeight = (int)(_whiteKeyHeight * 0.65);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_clientRect != ClientRectangle)
                UpdateMeasurements();

            var g = e.Graphics;

            var whiteBrush = new SolidBrush(Color.White);
            var blackBrush = new SolidBrush(Color.Black);
            var yellowBrush = new SolidBrush(Color.Bisque);
            var grayBrush = new SolidBrush(Color.DarkGray);
            var blackPen = new Pen(Color.Black);
            var font = new Font(FontFamily.GenericSansSerif, _whiteKeyWidth / 3, FontStyle.Regular, GraphicsUnit.Pixel);
            var stringFormat = new StringFormat(StringFormatFlags.NoWrap)
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Far
            };

            // Fill the background and the active keys
            g.FillRectangle(whiteBrush, _clientRect);
            for (var i = 0; i < WhiteKeys; i++)
            {
                var brush = (SolidBrush)null;

                if (i == MiddleC)
                    brush = yellowBrush;

                var down = _keyDown[NoteIndex(i, false)];
                if (down != 0)
                    brush = BrushFromDownPressure(down, Color.White);

                if (brush == null)
                    continue;

                g.FillRectangle(brush, Offset(WhiteKeyRect(i), 0, 1, 0, -2));
            }

            // Draw outline of keyboard and keys
            g.DrawRectangle(blackPen, new Rectangle(0, 0, _clientRect.Width - 1, _clientRect.Height - 1));

            for (var i = 0; i < WhiteKeys; i++)
            {
                var r = WhiteKeyRect(i);

                if ((i - 2) % 7 == 0)
                    g.DrawString("C" + (i / 7), font, grayBrush, Offset(r, 0, 0, 0, -_whiteKeyHeight / 16), stringFormat);

                g.DrawLine(blackPen, r.Right, r.Top, r.Right, r.Bottom);
            }

            // Draw each black key
            for (var i = 0; i < WhiteKeys; i++)
            {
                var r = BlackKeyRect(i);
                if (r == null)
                    continue;

                var down = _keyDown[NoteIndex(i, true)];
                if (down == 0)
                    g.FillRectangle(blackBrush, r.Value);
                else
                {
                    g.DrawRectangle(blackPen, r.Value);
                    g.FillRectangle(BrushFromDownPressure(down, Color.Black), Offset(r.Value, 1, 1, -1, -1));
                }
            }
        }

        private static SolidBrush BrushFromDownPressure(int down, Color normal)
        {
            var mix = (down + 32) / 160f;
            var mixinv = 1 - mix;
            var downColor = Color.DodgerBlue;

            return new SolidBrush(Color.FromArgb(
              (int)(normal.R * mixinv + downColor.R * mix),
              (int)(normal.G * mixinv + downColor.G * mix),
              (int)(normal.B * mixinv + downColor.B * mix)
            ));
        }

        private int MidiNoteFromClick(Point point)
        {
            // Did we click a black key?
            for (var i = 0; i < WhiteKeys; i++)
            {
                var r = BlackKeyRect(i);
                if (r != null && r.Value.Contains(point))
                    return NoteIndex(i, true);
            }

            // Did we click a white key?
            for (var i = 0; i < WhiteKeys; i++)
            {
                var r = WhiteKeyRect(i);
                if (r.Contains(point))
                    return NoteIndex(i, false);
            }

            return -1;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button != MouseButtons.Left)
                return;

            var note = MidiNoteFromClick(e.Location);
            if (note == -1)
                return;

            NoteManager.TurnNoteOn(note, 0x50);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button != MouseButtons.Left)
                return;

            var note = MidiNoteFromClick(e.Location);
            if (note == -1)
                return;

            NoteManager.TurnNoteOff(note);
        }
    }
}
