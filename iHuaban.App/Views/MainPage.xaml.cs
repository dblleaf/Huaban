using iHuaban.App.ViewModels;
using iHuaban.Core;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.Views
{
    public sealed partial class MainPage : Page, IPage<MainViewModel>
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

    }
}
