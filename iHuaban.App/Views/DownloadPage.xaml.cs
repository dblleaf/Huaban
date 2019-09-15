using iHuaban.App.ViewModels;
using iHuaban.Core;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.Views
{
    public sealed partial class DownloadPage : Page, IView<DownloadViewModel>
    {
        public DownloadPage()
        {
            this.InitializeComponent();
        }
    }
}
