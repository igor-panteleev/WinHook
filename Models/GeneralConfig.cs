using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using WinHook.Utils;

namespace WinHook.Models
{
    public class GeneralConfig: ObservableObject
    {
        private ShorcutSwitch _enableShortcut;
        private int? shortcutId;
        private bool _isEnabled;
        private bool _minimizeToTray;
        private bool _startMinimized;
        private bool _startWithWindows;

        public ShorcutSwitch EnableShortcut
        {
            get
            {
                return _enableShortcut;
            }
            set
            {
                if (_enableShortcut == value) return;
                _enableShortcut = value;
                if (shortcutId != null)
                {
                    HotKeyManager.UnregisterHotKey((int)shortcutId);
                }
                if (_enableShortcut != null)
                {
                    shortcutId = HotKeyManager.RegisterHotKey(_enableShortcut.Keys, _enableShortcut.ModifierKeys);
                }
                RaisePropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled == value) return;
                _isEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool MinimizeToTray
        {
            get { return _minimizeToTray; }
            set
            {
                if (_minimizeToTray == value) return;
                _minimizeToTray = value;
                RaisePropertyChanged();
            }
        }

        public bool StartMinimized
        {
            get { return _startMinimized; }
            set
            {
                if (_startMinimized == value) return;
                _startMinimized = value;
                RaisePropertyChanged();
            }
        }

        public bool StartWithWindows
        {
            get { return _startWithWindows; }
            set
            {
                if (_startWithWindows == value) return;
                _startWithWindows = value;
                RaisePropertyChanged();
            }
        }

        public void ClearEnableShortcutSwitch()
        {
            EnableShortcut = null;
        }

        private void InstallUninstallOnStartUp(bool install)
        {
            try
            {
                var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                var curAssembly = Assembly.GetExecutingAssembly();

                if (install)
                {
                    key.SetValue(curAssembly.GetName().Name, curAssembly.Location);
                }
                else
                {
                    key.DeleteValue(curAssembly.GetName().Name);
                }
            }
            catch (Exception e)
            {
                StartWithWindows = !install;
                var message = install ? "Can't add key to registry" : "Can't remove key from registry";
                Debug.WriteLine("{0} {1}", message, e.Message);
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand ClearEnableShortcutSwitchCommand
        {
            get { return new RelayCommand(ClearEnableShortcutSwitch); }
        }

        public ICommand InstallUninstallOnStartUpCommand
        {
            get { return new RelayCommand<bool>(InstallUninstallOnStartUp); }
        }
    }
}
