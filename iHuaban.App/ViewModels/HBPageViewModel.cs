using iHuaban.App.Models;
using iHuaban.Core.Models;

namespace iHuaban.App.ViewModels
{
    public abstract class HBPageViewModel : PageViewModel
    {
        public Context Context { get; private set; }
        public HBPageViewModel(Context context)
        {
            this.Context = context;
        }
    }
}
