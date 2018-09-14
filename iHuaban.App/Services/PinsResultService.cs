using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHuaban.App.Models;
using iHuaban.Core;
using iHuaban.Core.Helpers;

namespace iHuaban.App.Services
{
    public abstract class PinsResultService : IPinsResultService
    {
        public string ResourceName { set; get; }
        private HttpHelper helper { set; get; } = new HttpHelper();

        public PinsResultService(string resourceName)
        {
            ResourceName = resourceName;
        }
        public abstract string GetApiUrl();
        public abstract string GetApiPinsUrl();

        public IEnumerable<Pin> GetPins(int limit = 0, long max = 0)
        {
            return GetPinsAsync(limit, max).Result;
        }

        public async Task<IEnumerable<Pin>> GetPinsAsync(int limit = 20, long max = 0)
        {
            List<KeyValuePair<string, long>> param = new List<KeyValuePair<string, long>>()
            {
                new KeyValuePair<string, long>("limit", limit),
                new KeyValuePair<string, long>("max", max)
            };
            var result = await helper.GetAsync<PinCollection>(GetApiPinsUrl() + param.ToQueryString());
            return result.Pins;
        }
    }
}
