using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MidiJunction.Classes;

namespace MidiJunction.Forms
{
    public partial class FormKeyboard : Form
    {
        private static readonly Dictionary<Keys, int> KeyNotes = new Dictionary<Keys, int>
        {
            // White keys
            { Keys.A, 0 },
            { Keys.S, 2 },
            { Keys.D, 4 },
            { Keys.F, 5 },
            { Keys.G, 7 },
            { Keys.H, 9 },
            { Keys.J, 11 },
            { Keys.K, 12 },
            { Keys.L, 14 },

            // Black keys
            { Keys.W, 1 },
            { Keys.E, 3 },
            { Keys.T, 6 },
            { Keys.Y, 8 },
            { Keys.U, 10 },
            { Keys.O, 13 }
        };

        private readonly List<Keys> _active = new List<Keys>();

        public FormKeyboard()
        {
            InitializeComponent();
        }

        private void FormKeyboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void FormKeyboard_KeyDown(object sender, KeyEventArgs e)
        {
            if (!KeyNotes.TryGetValue(e.KeyCode, out var note))
                return;

            if (_active.Contains(e.KeyCode))
                return;

            e.Handled = true;
            _active.Add(e.KeyCode);
            NoteManager.TurnNoteOn(60 + note, 0x50);
        }

        private void FormKeyboard_KeyUp(object sender, KeyEventArgs e)
        {
            if (!KeyNotes.TryGetValue(e.KeyCode, out var note))
                return;

            NoteManager.TurnNoteOff(60 + note);
            _active.Remove(e.KeyCode);
        }

        [DllImport("user32.dll")]
        static extern short GetKeyState(int key);

        private static bool IsKeyDown(Keys key)
        {
            return (GetKeyState((int)key) & 128) != 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach(var key in _active.ToList())
                if (!IsKeyDown(key))
                {
                    NoteManager.TurnNoteOff(60 + KeyNotes[key]);
                    _active.Remove(key);
                }
        }

        private void FormKeyboard_VisibleChanged(object sender, EventArgs e)
        {
            timer1.Enabled = Visible;
        }
    }
}
