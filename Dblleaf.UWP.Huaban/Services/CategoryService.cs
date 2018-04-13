using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dblleaf.UWP.Huaban.Services
{
    using Dblleaf.UWP.Huaban.Helpers;
    using Models;
    public class CategoryService : ServiceBase
    {
        public CategoryService(HbHttpHelper httpHelper)
            : base(httpHelper) { }

        public async Task<List<Category>> GetCategoryList()
        {
            try
            {
                string json = await HttpHelper.Get(Constants.API_CATEGORY);
                var obj = JObject.Parse(json);
                return Category.ParseList(obj["categories"] as JArray);
            }
            catch (Exception ex)
            {
                string aaa = ex.Message;
            }
            return new List<Category>();
        }

        public async Task<List<Pin>> GetCategoryPinList(string nav_link, int limit = 20, long max = 0)
        {
            string json = await GetCategoryPinStrng(nav_link, limit, max);
            var obj = JObject.Parse(json);
            return Pin.ParseList(obj["pins"] as JArray);
        }

        public async Task<string> GetCategoryPinStrng(string nav_link, int limit = 20, long max = 0)
        {
            string maxStr = max.ToString();
            if (max <= 0)
                maxStr = "";
            nav_link = nav_link.TrimStart('/');
            return await HttpHelper.Get($"{Constants.API_MAIN}{nav_link}?limit={limit}&max={maxStr}");
        }
    }
}
