using iHuaban.App.ViewModels;
using iHuaban.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.Views
{

    public sealed partial class ShellPage : Page, IPage<ShellViewModel>
    {
        public ShellPage()
        {
            this.InitializeComponent();
            Window.Current.SetTitleBar(coreTitle);
        }
    }
}
