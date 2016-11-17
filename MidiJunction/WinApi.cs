using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MidiJunction
{
    public class WinApi
    {
        // ReSharper disable InconsistentNaming

        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        public static readonly IntPtr HWND_TOP = new IntPtr(0);
        public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        public static class HWND
        {
            public static IntPtr
            NoTopMost = new IntPtr(-2),
            TopMost = new IntPtr(-1),
            Top = new IntPtr(0),
            Bottom = new IntPtr(1);
        }

        public const int AC_SRC_ALPHA = 1;
        public const int AC_SRC_OVER = 0;
        public const int LWA_ALPHA = 2;
        public const int LWA_COLORKEY = 1;
        public const int MOD_ALT = 1;
        public const int MOD_CONTROL = 2;
        public const int MOD_SHIFT = 4;
        public const int MOD_WIN = 8;
        public const int ULW_ALPHA = 2;
        public const int ULW_COLORKEY = 1;
        public const int ULW_OPAQUE = 4;
        public const int WM_ACTIVATE = 0x0006;
        public const int WM_HOTKEY = 0x0312;
        public const int WS_EX_LAYERED = 0x00080000;
        public const int WS_EX_TRANSPARENT = 0x00000020;

        public class SWP
        {
            public static readonly uint
              NOSIZE = 0x0001,
              NOMOVE = 0x0002,
              NOZORDER = 0x0004,
              NOREDRAW = 0x0008,
              NOACTIVATE = 0x0010,
              DRAWFRAME = 0x0020,
              FRAMECHANGED = 0x0020,
              SHOWWINDOW = 0x0040,
              HIDEWINDOW = 0x0080,
              NOCOPYBITS = 0x0100,
              NOOWNERZORDER = 0x0200,
              NOREPOSITION = 0x0200,
              NOSENDCHANGING = 0x0400,
              DEFERERASE = 0x2000,
              ASYNCWINDOWPOS = 0x4000;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, IntPtr pptDst, IntPtr psize, IntPtr hdcSrc, IntPtr pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);
    }
}
