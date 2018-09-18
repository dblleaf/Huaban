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
    public abstract class PinsResultService<T> : HbService<T>, IPinsResultService<T> where T : new()
    {
        private HbService<PinCollection> PinResultService { set; get; }
        public PinsResultService(string resourceName, HttpHelper helper)
            : base(resourceName, helper)
        {
            PinResultService = new HbService<PinCollection>($"{resourceName}{Constants.ApiPinsName}/", helper);
        }

        public PinCollection GetPins(int limit = 0, long max = 0)
        {
            return GetPinsAsync(limit, max).Result;
        }

        public async Task<PinCollection> GetPinsAsync(int limit = 20, long max = 0)
        {
            return await PinResultService.GetAsync(limit, max);
        }
    }
}
