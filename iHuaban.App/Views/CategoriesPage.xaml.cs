using iHuaban.App.ViewModels;
using iHuaban.Core;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.Views
{

    public sealed partial class CategoriesPage : Page, IView<CategoriesViewModel>
    {
        public CategoriesPage()
        {
            this.InitializeComponent();
        }
    }
}
