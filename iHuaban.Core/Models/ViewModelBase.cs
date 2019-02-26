using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace iHuaban.Core.Models
{
    public class ViewModelBase : ObservableObject
    {
        public Setting Setting { get; private set; } = Setting.Instance();

        
        public virtual async Task InitAsync()
        {
            await Task.FromResult(0);
        }
    }
}
