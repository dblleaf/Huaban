using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Huaban.UWP.Services
{
    using Models;
    public class CategoryService : ServiceBase
    {
        public CategoryService(IClient client)
            : base(client) { }

        public async Task<List<Category>> GetCategoryList()
        {
            try
            {
                string json = await Get(Constants.API_CATEGORY);
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
            return await Get($"{Constants.API_MAIN}{nav_link}?limit={limit}&max={maxStr}");
        }
    }
}
