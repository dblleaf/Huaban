using iHuaban.App.Models;
using iHuaban.App.Services;

namespace iHuaban.App.ViewModels
{
    public class FindViewModel : ListViewModel<Category>
    {
        public FindViewModel(IHbService<ModelCollection<Category>> pinsResultService)
           : base(pinsResultService, false)
        {
        }
    }
}
