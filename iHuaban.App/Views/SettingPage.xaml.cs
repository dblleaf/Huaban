using iHuaban.App.ViewModels;
using iHuaban.Core;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.Views
{
    public sealed partial class SettingPage : Page, IView<SettingViewModel>
    {
        public SettingPage()
        {
            this.InitializeComponent();
        }

    }
}
