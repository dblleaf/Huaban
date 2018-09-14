using Dblleaf.UWP.Huaban.Services.Navigation;
using Dblleaf.UWP.Huaban.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Dblleaf.UWP.Huaban.Views
{
    public sealed partial class MainView : Page, IPageWithViewModel<MainViewModel>
    {
        public MainView()
        {
            this.InitializeComponent();
        }

        public MainViewModel ViewModel { get; set; }

        public void UpdateBindings()
        {
        }
    }
}
