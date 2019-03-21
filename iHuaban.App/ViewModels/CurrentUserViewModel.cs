using iHuaban.App.Models;
using iHuaban.Core.Models;

namespace iHuaban.App.ViewModels
{
    public class CurrentUserViewModel : ViewModelBase
    {
        public Context Context { get; private set; }
        public override string TemplateName => Constants.TemplateCurrentUser;
        public CurrentUserViewModel(Context context)
        {
            this.Context = context;
        }
    }
}
