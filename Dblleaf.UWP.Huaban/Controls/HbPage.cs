using Dblleaf.UWP.Huaban.Services;
using Dblleaf.UWP.Huaban.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Dblleaf.UWP.Huaban.Controls
{
    public abstract class HbPage<T> : Page where T : ViewModelBase
    {
        public T ViewModel { private set; get; }
        public HbPage()
        {
            ViewModel = ServiceLocator.Resolve<T>();
            this.DataContext = ViewModel;
        }
    }
}
