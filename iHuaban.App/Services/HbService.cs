using iHuaban.App.Models;
using iHuaban.Core;
using iHuaban.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public class HbService<T> : IHbService<T> where T : new()
    {
        public string ResourceName { set; get; }
        protected HttpHelper Helper { private set; get; }
        public HbService(string resourceName, HttpHelper helper = null)
        {
            Helper = helper ?? new HttpHelper();
            ResourceName = resourceName;
        }

        public virtual string GetApiUrl()
        {
            return $"{Constants.ApiBase}{ResourceName}";
        }

        public async Task<T> GetAsync(int limit = 0, long max = 0)
        {
            List<KeyValuePair<string, long>> param = new List<KeyValuePair<string, long>>()
            {
                new KeyValuePair<string, long>("limit", limit),
                new KeyValuePair<string, long>("max", max)
            };
            var result = await Helper.GetAsync<T>(GetApiUrl() + param.ToQueryString());
            return result;
        }

        public T Get(int limit = 0, long max = 0)
        {
            return GetAsync(limit, max).Result;
        }
    }
}
