using iHuaban.App.Services;
using iHuaban.App.ViewModels;
using iHuaban.Core;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.Views
{
    public sealed partial class AboutPage : Page, IPage<AboutViewModel>
    {
        public AboutPage()
        {
            this.InitializeComponent();
        }

        private AboutViewModel vm;
        public AboutViewModel ViewModel
        {
            get => vm ?? (vm = Locator.ResolveObject<AboutViewModel>());
        }
    }
}
