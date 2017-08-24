using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using WinHook.Utils;
using TextBox = System.Windows.Controls.TextBox;

namespace WinHook.Controls
{

    public class SysKeyInputBox : TextBox
    {
        static SysKeyInputBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SysKeyInputBox), new FrameworkPropertyMetadata(typeof(SysKeyInputBox)));
        }

        public SysKeyInputBox()
        {
            KeyHook.KeyCapture += KeyHookOnKeyCapture;
        }

        private void KeyHookOnKeyCapture(object sender, KeyCaptureEventArgs e)
        {
            if (!IsFocused) return;

            Keys = e.Key;
            e.Handled = true;

            CommandManager.InvalidateRequerySuggested();

        }

        public static readonly DependencyProperty KeysProperty = DependencyProperty.Register(
            "Keys", typeof(Keys?), typeof(SysKeyInputBox), new FrameworkPropertyMetadata(new Keys(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, FillText)
            );

        private static void FillText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var invoker = d as SysKeyInputBox;

            var newValue = e.NewValue == null ? string.Empty : e.NewValue.ToString();
            invoker.Text = newValue;
        }

        public Keys Keys
        {
            get
            {
                return (Keys)GetValue(KeysProperty);
            }
            set
            {
                SetValue(KeysProperty, value);
            }
        }
    }
}
