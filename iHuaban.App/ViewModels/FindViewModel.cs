using iHuaban.App.Models;
using iHuaban.App.Services;

namespace iHuaban.App.ViewModels
{
    public class FindViewModel : ListViewModel<Category>
    {
        public FindViewModel(IService<ModelCollection<Category>> service)
           : base(service, false)
        {
        }
    }
}
