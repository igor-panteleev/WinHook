using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using WinHook.Models;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using KeyEventHandler = System.Windows.Input.KeyEventHandler;
using TextBox = System.Windows.Controls.TextBox;

namespace WinHook.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WinHook.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WinHook.Controls;assembly=WinHook.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:KeyInputBox/>
    ///
    /// </summary>
    public class KeyInputBox : TextBox
    {
        static KeyInputBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KeyInputBox), new FrameworkPropertyMetadata(typeof(KeyInputBox)));
            EventManager.RegisterClassHandler(typeof(KeyInputBox), Keyboard.PreviewKeyDownEvent, new KeyEventHandler(OnPreviewKeyDownEvent));
        }

        public static readonly DependencyProperty ShorcutModeProperty = DependencyProperty.Register(
            "ShorcutMode", typeof(bool), typeof(KeyInputBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );
        public static readonly DependencyProperty KeysProperty = DependencyProperty.Register(
            "Keys", typeof(Keys?), typeof(KeyInputBox), new FrameworkPropertyMetadata(new Keys(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, FillText)
            );
        public static readonly DependencyProperty ShorcutProperty = DependencyProperty.Register(
            "Shorcut", typeof(ShorcutSwitch), typeof(KeyInputBox), new FrameworkPropertyMetadata(new ShorcutSwitch(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, FillText)
            );

        private static void FillText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var invoker = d as KeyInputBox;

            var newValue = e.NewValue == null ? string.Empty : e.NewValue.ToString();
            invoker.Text = newValue;
        }

        public bool ShorcutMode
        {
            get
            {
                return (bool)GetValue(ShorcutModeProperty);
            }
            set
            {
                SetValue(ShorcutModeProperty, value);
            }
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

        public ShorcutSwitch Shorcut
        {
            get
            {
                return (ShorcutSwitch)GetValue(ShorcutProperty);
            }
            set
            {
                SetValue(ShorcutProperty, value);
            }
        }


        public static readonly RoutedEvent InvertCallEvent = EventManager.RegisterRoutedEvent(
            "InvertCall", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(KeyInputBox)
            );

        public event RoutedEventHandler InvertCall
        {
            add { AddHandler(InvertCallEvent, value); }
            remove { RemoveHandler(InvertCallEvent, value); }
        }

        private void OnInvertCall()
        {
            var args = new RoutedEventArgs(InvertCallEvent);
            RaiseEvent(args);
        }

        static void OnPreviewKeyDownEvent(object sender, KeyEventArgs e)
        {
            var invoker = sender as KeyInputBox;

            // The text box grabs all input.
            e.Handled = true;

            if (invoker.ShorcutMode)
            {
                // Fetch the actual shortcut key.
                var key = (e.Key == Key.System ? e.SystemKey : e.Key);

                // Ignore modifier keys.
                if (key == Key.LeftShift || key == Key.RightShift
                    || key == Key.LeftCtrl || key == Key.RightCtrl
                    || key == Key.LeftAlt || key == Key.RightAlt
                    || key == Key.LWin || key == Key.RWin)
                {
                    // do nothing
                }
                else
                {
                    invoker.Shorcut = new ShorcutSwitch((Keys)KeyInterop.VirtualKeyFromKey(key), Keyboard.Modifiers);
                }
            }
            else
            {
                invoker.Keys = (Keys) KeyInterop.VirtualKeyFromKey(e.Key);
            }

            invoker.OnInvertCall();
        }
    }
}
