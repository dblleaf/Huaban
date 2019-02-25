using System.Threading.Tasks;

namespace iHuaban.Core.Models
{
    public class ViewModelBase : ObservableObject
    {
        public ViewModelBase()
        {
        }

        public virtual async Task InitAsync()
        {
        }
    }
}
