using iHuaban.App.ViewModels;
using iHuaban.Core;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.Views
{
    public sealed partial class FindPage : Page, IView<FindViewModel>
    {
        public FindPage()
        {
            this.InitializeComponent();
        }
    }
}
