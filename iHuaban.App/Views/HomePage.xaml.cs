using iHuaban.App.ViewModels;
using iHuaban.Core;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.Views
{
    public sealed partial class HomePage : Page, IView<HomeViewModel>
    {
        public HomePage()
        {
            this.InitializeComponent();
        }
    }
}
