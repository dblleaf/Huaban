using iHuaban.App.Models;
using iHuaban.App.Services;

namespace iHuaban.App.ViewModels
{
    public class FindViewModel : ListViewModel<Category, CategoryCollection>
    {
        public FindViewModel(IServiceProvider service)
           : base(service, false)
        {
        }

        protected override string GetApiUrl()
        {
            return Constants.ApiCategories;
        }
    }
}
