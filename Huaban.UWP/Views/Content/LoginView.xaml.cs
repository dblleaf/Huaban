using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.Foundation.Metadata;
namespace Huaban.UWP.Views
{
    public sealed partial class LoginView : UserControl
    {
        private Frame Shell;
        public LoginView()
        {
            this.InitializeComponent();
            this.Loaded += LoginView_Loaded;
            this.Unloaded += LoginView_Unloaded;
        }

        private void LoginView_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Shell.SizeChanged -= Shell_SizeChanged;
        }

        private void LoginView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Focus(FocusState.Programmatic);
            
            Shell = Window.Current.Content as Frame;
            this.Width = Shell.ActualWidth;
            this.Height = Shell.ActualHeight;
            Shell.SizeChanged += Shell_SizeChanged;
        }

        private void Shell_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Width = Shell.ActualWidth;
            this.Height = Shell.ActualHeight;
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Focus(FocusState.Programmatic);
        }
    }
}
