using iHuaban.App.Models;
using iHuaban.App.Services;

namespace iHuaban.App.ViewModels
{
    public class FindViewModel : ListViewModel<CategoryCollection, Category>
    {
        public FindViewModel(IHbService<CategoryCollection> pinsResultService)
           : base(pinsResultService, false)
        {
        }
    }
}
