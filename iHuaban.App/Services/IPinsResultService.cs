using iHuaban.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public interface IPinsResultService<T> : IHbService<T> where T : new()
    {
        PinCollection GetPins(int limit = 0, long max = 0);
        Task<PinCollection> GetPinsAsync(int limit = 0, long max = 0);
    }
}
