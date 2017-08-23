using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;
using WinHook.Models;
using WinHook.Utils;

namespace WinHook.ViewModels
{
    public class WinHookViewModel : ObservableObject
    {
        private const string ConfigFileName = "config.xml";

        private Config _config;

        public WinHookViewModel()
        {
            _config = LoadConfig();
            HotKeyManager.HotKeyPressed += HotKeyManager_HotKeyPressed;
            KeyHook.KeyCapture += KeyHook_KeyCapture;
            EventProxy<PropertyChangedEventArgs>.EventCaptured += OnEventCaptured;
        }

        private void OnEventCaptured(object sender, PropertyChangedEventArgs e)
        {
            var isIgnore = sender.GetType().GetProperty(e.PropertyName).GetCustomAttributes(false).Any(a => a is XmlIgnoreAttribute);
            if (isIgnore) return;

            SaveConfig();
        }
        private void SaveConfig()
        {
            try
            {
                using (var fs = File.Open(ConfigFilePath, FileMode.Create))
                    (new XmlSerializer(typeof(Config))).Serialize(fs, _config);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine("Can't save {0}; {1};", ConfigFilePath, e.Message);
                return;
            }
            Debug.WriteLine("Config file saved {0};", ConfigFilePath);
        }

        public Config LoadConfig()
        {
            try
            {
                using (var fs = File.Open(ConfigFilePath, FileMode.Open))
                    return (Config)(new XmlSerializer(typeof(Config))).Deserialize(fs);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Can't load {0}; {1}", ConfigFilePath, e);
                return new Config();
            }
        }

        private static string ConfigFilePath
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName); }
        }

        public Config Config
        {
            get { return _config; }
            set
            {
                if (_config == value) return;
                _config = value;
                RaisePropertyChanged();
            }
        }

        //public ICommand SaveCommand
        //{
        //    get { return new RelayCommand(SaveConfig); }
        //}

        private void HotKeyManager_HotKeyPressed(object sender, HotKeyEventArgs e)
        {
            if (e.Key == Config.GeneralConfig.EnableShortcut.Keys && e.Modifiers == Config.GeneralConfig.EnableShortcut.ModifierKeys)
            {
                Config.GeneralConfig.IsEnabled = !Config.GeneralConfig.IsEnabled;
            }
        }

        private void KeyHook_KeyCapture(object sender, KeyCaptureEventArgs e)
        {
            if (Config.KeyBlockConfig.BlockedKeys.Any(key => key == e.Key))
            {
                e.Handled = true;
            }
        }
    }
}
