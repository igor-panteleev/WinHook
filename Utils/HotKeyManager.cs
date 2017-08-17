using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace WinHook.Utils
{
    public static class HotKeyManager
    {
        [DllImport("user32", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private static int _id = 0;
        private const int WM_HOTKEY = 0x312;
        private const int NoRepeat = 0x4000;

        private static volatile MessageWindow _wnd;
        private static volatile IntPtr _hwnd;
        private static readonly ManualResetEvent WindowReadyEvent = new ManualResetEvent(false);

        delegate void RegisterHotKeyDelegate(IntPtr hwnd, int id, uint modifiers, uint key);
        delegate void UnRegisterHotKeyDelegate(IntPtr hwnd, int id);

        public static event EventHandler<HotKeyEventArgs> HotKeyPressed;

        public static int RegisterHotKey(Keys key, ModifierKeys modifiers, bool noRepeat = true)
        {
            WindowReadyEvent.WaitOne();
            var id = Interlocked.Increment(ref _id);
            var combinedModifiers = noRepeat ? (uint)modifiers | NoRepeat : (uint)modifiers;
            _wnd.Invoke(new RegisterHotKeyDelegate(RegisterHotKeyInternal), _hwnd, id, combinedModifiers, (uint)key);
            return id;
        }

        public static void UnregisterHotKey(int id)
        {
            _wnd.Invoke(new UnRegisterHotKeyDelegate(UnRegisterHotKeyInternal), _hwnd, id);
        }

        private static void RegisterHotKeyInternal(IntPtr hwnd, int id, uint modifiers, uint key)
        {
            RegisterHotKey(hwnd, id, modifiers, key);
        }

        private static void UnRegisterHotKeyInternal(IntPtr hwnd, int id)
        {
            UnregisterHotKey(_hwnd, id);
        }

        private static void OnHotKeyPressed(HotKeyEventArgs e)
        {
            if (HotKeyPressed != null)
            {
                HotKeyPressed(null, e);
            }
        }

        static HotKeyManager()
        {
            var messageLoop = new Thread(delegate() { Application.Run(new MessageWindow()); })
            {
                Name = "MessageLoopThread",
                IsBackground = true
            };
            messageLoop.Start();
        }

        private class MessageWindow : Form
        {
            public MessageWindow()
            {
                _wnd = this;
                _hwnd = Handle;
                WindowReadyEvent.Set();
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_HOTKEY)
                {
                    var e = new HotKeyEventArgs(m.LParam);
                    OnHotKeyPressed(e);
                }

                base.WndProc(ref m);
            }

            protected override void SetVisibleCore(bool value)
            {
                // Ensure the window never becomes visible
                base.SetVisibleCore(false);
            }
        }
    }


    public class HotKeyEventArgs : EventArgs
    {
        public readonly Keys Key;
        public readonly ModifierKeys Modifiers;

        public HotKeyEventArgs(Keys key, ModifierKeys modifiers)
        {
            Key = key;
            Modifiers = modifiers;
        }

        public HotKeyEventArgs(IntPtr hotKeyParam)
        {
            var param = (uint)hotKeyParam.ToInt64();
            Key = (Keys)((param & 0xffff0000) >> 16);
            Modifiers = (ModifierKeys)(param & 0x0000ffff);
        }
    }
}
