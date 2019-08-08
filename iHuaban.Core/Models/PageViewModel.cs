using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace iHuaban.Core.Models
{
    public abstract class PageViewModel : ViewModelBase
    {
        public virtual async Task InitAsync(NavigationEventArgs e)
        {
            await Task.FromResult(0);
        }
    }
}
