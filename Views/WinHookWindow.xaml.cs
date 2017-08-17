using System.Windows;
using WinHook.ViewModels;
using WinHook.Utils;

namespace WinHook.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var window = (MainWindow) sender;
            var vm = (WinHookViewModel)window.DataContext;

            if (vm.Config.GeneralConfig.MinimizeToTray)
                MinimizeToTray.Enable(this);

            if (vm.Config.GeneralConfig.StartMinimized)
                WindowState = WindowState.Minimized;
        }
    }
}
