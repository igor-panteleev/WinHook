using System;

namespace WinHook.Models
{
    [Serializable]
    public class Config: ObservableObject
    {
        private GeneralConfig _generalConfig = new GeneralConfig();
        private KeyBlockConfig _keyBlockConfig = new KeyBlockConfig();

        public GeneralConfig GeneralConfig
        {
            get { return _generalConfig; }
            set
            {
                if (_generalConfig == value) return;
                _generalConfig = value;
                RaisePropertyChanged();
            }
        }

        public KeyBlockConfig KeyBlockConfig
        {
            get { return _keyBlockConfig; }
            set
            {
                if (_keyBlockConfig == value) return;
                _keyBlockConfig = value;
                RaisePropertyChanged();
            }
        }
    }
}
