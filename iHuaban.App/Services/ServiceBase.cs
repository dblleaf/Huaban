using iHuaban.Core;
using iHuaban.Core.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public class Service<T> : IService<T>
    {
        protected IHttpHelper HttpHelper { get; private set; }
        public Service(IHttpHelper httpHelper)
        {
            this.HttpHelper = httpHelper;
        }

        public T Get(string uri, int limit = 0, long max = 0)
        {
            return GetAsync(uri, limit, max).Result;
        }

        public async Task<T> GetAsync(string uri, int limit = 0, long max = 0)
        {
            List<KeyValuePair<string, long>> param = new List<KeyValuePair<string, long>>()
            {
                new KeyValuePair<string, long>("limit", limit),
                new KeyValuePair<string, long>("max", max)
            };
            var result = await HttpHelper.GetAsync<T>(uri + param.ToQueryString());
            return result;
        }
    }
}
