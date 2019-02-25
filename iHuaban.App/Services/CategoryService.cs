using iHuaban.App.Models;
using iHuaban.Core.Helpers;

namespace iHuaban.App.Services
{
    public class CategoryService : Service<CategoryCollection>, ICategoryService
    {
        public CategoryService(IHttpHelper httpHelper) : base(httpHelper)
        {
        }
    }
}
