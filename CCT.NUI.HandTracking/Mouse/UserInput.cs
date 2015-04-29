using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace CCT.NUI.HandTracking.Mouse
{
    internal class UserInput
    {
        public static void SetCursorPositionAbsolute(int x, int y)
        {                            
            var dx = x * (65536 / System.Windows.SystemParameters.PrimaryScreenWidth);
            var dy = y * (65536 / System.Windows.SystemParameters.PrimaryScreenHeight);
            MouseInput((int) dx, (int) dy, MouseEventFlags.MOUSEEVENTF_MOVE | MouseEventFlags.MOUSEEVENTF_ABSOLUTE);
        }

        public static void MouseDown()
        {
            MouseInput(0, 0, MouseEventFlags.MOUSEEVENTF_LEFTDOWN);
        }

        public static void MouseUp()
        {
            MouseInput(0, 0, MouseEventFlags.MOUSEEVENTF_LEFTUP);
        }

        private static void MouseInput(int dx, int dy, MouseEventFlags flags) 
        {
            INPUT input = new INPUT();
            input.type = SendInputEventType.InputMouse;
            input.mkhi.mi.dwExtraInfo = IntPtr.Zero;
            input.mkhi.mi.dx = dx;
            input.mkhi.mi.dy = dy;
            input.mkhi.mi.time = 0;
            input.mkhi.mi.mouseData = 0;
            input.mkhi.mi.dwFlags = flags;
            var result = SendInput(1, ref input, Marshal.SizeOf(typeof(INPUT)));
            if (result == 0)
            {
                Debug.WriteLine(Marshal.GetLastWin32Error());
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        [StructLayout(LayoutKind.Sequential)]
        struct MouseInputData
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public MouseEventFlags dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct MouseKeybdhardwareInputUnion
        {
            [FieldOffset(0)]
            public MouseInputData mi;

            [FieldOffset(0)]
            public KEYBDINPUT ki;

            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public SendInputEventType type;
            public MouseKeybdhardwareInputUnion mkhi;
        }

        enum SendInputEventType : int
        {
            InputMouse,
            InputKeyboard,
            InputHardware
        }

        [Flags]
        enum MouseEventFlags : uint
        {
            MOUSEEVENTF_MOVE = 0x0001,
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            MOUSEEVENTF_LEFTUP = 0x0004,
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            MOUSEEVENTF_RIGHTUP = 0x0010,
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            MOUSEEVENTF_MIDDLEUP = 0x0040,
            MOUSEEVENTF_XDOWN = 0x0080,
            MOUSEEVENTF_XUP = 0x0100,
            MOUSEEVENTF_WHEEL = 0x0800,
            MOUSEEVENTF_VIRTUALDESK = 0x4000,
            MOUSEEVENTF_ABSOLUTE = 0x8000
        }
    }
}
