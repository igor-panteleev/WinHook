using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Serialization;

namespace WinHook.Models
{
    public class KeyBlockConfig: ObservableObject
    {
        private ObservableCollection<Keys> _blockedKeys = new ObservableCollection<Keys>();
        private Keys? _tmpKey;

        public KeyBlockConfig()
        {
            // ObservableCollection won't throw RaisePropertyChanged on change
            // So we doo it manually
            BlockedKeys.CollectionChanged += BlockedKeys_CollectionChanged;
        }

        void BlockedKeys_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("BlockedKeys");
        }

        public ObservableCollection<Keys> BlockedKeys
        {
            get { return _blockedKeys; }
            set
            {
                if (_blockedKeys == value) return;
                _blockedKeys = value;
                RaisePropertyChanged();
            }
        }

        [XmlIgnore]
        public Keys? TmpAddKeys
        {
            get { return _tmpKey; }
            set
            {
                if (_tmpKey == value) return;
                _tmpKey = value;
                RaisePropertyChanged();
            }
        }

        public ICommand AddKeyCommand
        {
            get { return new RelayCommand<Keys>(AddKey, CanAddKey); }
        }

        public void AddKey(Keys key)
        {
            BlockedKeys.Add(key);
            TmpAddKeys = null;
        }

        public bool CanAddKey(Keys key)
        {
            return (key != Keys.None && !BlockedKeys.Contains(key));
        }

        public ICommand RemoveKeyCommand
        {
            get { return new RelayCommand<Keys>(RemoveKey, CanRemoveKey); }
        }

        public void RemoveKey(Keys key)
        {
            BlockedKeys.Remove(key);
        }

        public bool CanRemoveKey(Keys key)
        {
            return (key != Keys.None && BlockedKeys.Contains(key));
        }
    }
}
