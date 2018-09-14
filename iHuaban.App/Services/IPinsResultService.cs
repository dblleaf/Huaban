using iHuaban.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public interface IPinsResultService
    {
        IEnumerable<Pin> GetPins(int limit = 0, long max = 0);
        Task<IEnumerable<Pin>> GetPinsAsync(int limit = 0, long max = 0);
    }
}
