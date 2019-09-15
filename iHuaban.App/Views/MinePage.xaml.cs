using iHuaban.App.ViewModels;
using iHuaban.Core;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.Views
{
    public sealed partial class MinePage : Page, IView<MineViewModel>
    {
        public MinePage()
        {
            this.InitializeComponent();
        }
    }
}
