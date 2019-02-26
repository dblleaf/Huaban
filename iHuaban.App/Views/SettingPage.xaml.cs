using iHuaban.App.Services;
using iHuaban.App.ViewModels;
using iHuaban.Core;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.Views
{
    public sealed partial class SettingPage : Page, IPage<SettingViewModel>
    {
        public SettingPage()
        {
            this.InitializeComponent();
        }

        private SettingViewModel vm;
        public SettingViewModel ViewModel
        {
            get => vm ?? (vm = Locator.ResolveObject<SettingViewModel>());
        }
    }
}
