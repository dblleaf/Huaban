using System.Threading.Tasks;

namespace iHuaban.Core.Models
{
    public abstract class ViewModelBase : ObservableObject
    {
        public ViewModelBase()
        {
        }

        public virtual async Task InitAsync()
        {
        }
    }
}
