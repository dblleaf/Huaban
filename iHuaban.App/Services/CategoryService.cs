using iHuaban.App.Models;
using iHuaban.Core;
using iHuaban.Core.Helpers;

namespace iHuaban.App.Services
{
    public class CategoryService : PinsResultService<Category>
    {
        public CategoryService(string categoryName, HttpHelper httpHelper)
            : base($"categories/{categoryName}/", httpHelper)
        { }

        public override string GetApiUrl()
        {
            return Constants.ApiCategories;
        }
    }
}
