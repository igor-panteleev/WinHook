using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinHook.Utils
{
    public class KeyCaptureEventArgs : EventArgs
        {
            public Keys Key { get; private set; }
            public int Flags { get; private set; }
            public IntPtr Extra { get; private set; }
            public bool Handled { get; set; }

            public KeyCaptureEventArgs(Keys key, int flags, IntPtr extra)
            {
                Flags = flags;
                Extra = extra;
                Key = key;
                Handled = false;
            }
        }

    public delegate void KeyCaptyreEventHandler(object sender, KeyCaptureEventArgs e);

    public static class KeyHook
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Kbdllhookstruct
        {
            public Keys key;
            public int scanCode;
            public int flags;
            public int time;
            public IntPtr extra;
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public delegate void KeyCaptyreEventHandler(object sender, KeyCaptureEventArgs e);
        public static event KeyCaptyreEventHandler KeyCapture;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int id, LowLevelKeyboardProc callback, IntPtr hMod,
                                                      uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hook, int nCode, IntPtr wp, IntPtr lp);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string name);

        private static IntPtr _ptrHook;
        private static readonly LowLevelKeyboardProc ObjKeyboardProcess;

        static KeyHook()
        {
            ProcessModule objCurrentModule = Process.GetCurrentProcess().MainModule;
            ObjKeyboardProcess = CaptureKey;
            _ptrHook = SetWindowsHookEx(13, ObjKeyboardProcess, GetModuleHandle(objCurrentModule.ModuleName), 0);
        }

        public static void Clear()
        {
            if (_ptrHook == IntPtr.Zero) return;
            UnhookWindowsHookEx(_ptrHook);
            _ptrHook = IntPtr.Zero;
        }

        private static IntPtr CaptureKey(int nCode, IntPtr wp, IntPtr lp)
        {
            if (nCode >= 0)
            {
                var objKeyInfo = (Kbdllhookstruct)Marshal.PtrToStructure(lp, typeof(Kbdllhookstruct));
                var e = new KeyCaptureEventArgs(objKeyInfo.key, objKeyInfo.flags, objKeyInfo.extra);
                OnkeyCaptyre(e);
                if (e.Handled)
                {
                    return (IntPtr)1;
                }
            }
            return CallNextHookEx(_ptrHook, nCode, wp, lp);
        }

        private static void OnkeyCaptyre(KeyCaptureEventArgs e)
        {
            if (KeyCapture != null)
            {
                KeyCapture(null, e);
            }
        }

    }
}
