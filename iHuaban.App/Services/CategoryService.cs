using iHuaban.Core;

namespace iHuaban.App.Services
{
    public class CategoryService : PinsResultService
    {
        public CategoryService(string categoryName) 
            : base(categoryName)
        { }

        public override string GetApiUrl()
        {
            return Constants.ApiCategories;
        }

        public override string GetApiPinsUrl()
        {
            return GetApiUrl() + ResourceName;
        }
    }
}
