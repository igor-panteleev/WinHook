using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace WinHook.Models
{
    public class ObservableObject: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null) return;

            var eventArgs = new PropertyChangedEventArgs(propertyName);
            PropertyChanged(this, eventArgs);
            Debug.WriteLine(propertyName);
            EventProxy<PropertyChangedEventArgs>.CaptureEvent(this, eventArgs);
        }
    }
}
