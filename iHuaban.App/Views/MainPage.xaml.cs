using iHuaban.App.Services;
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
            this.Loaded += async (sender, e) =>
            {
                await ViewModel.InitAsync();
            };
        }

        private MainViewModel vm;
        public MainViewModel ViewModel
        {
            get => vm ?? (vm = Locator.ResolveObject<MainViewModel>());
        }
    }
}
