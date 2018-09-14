using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public interface IHbService<T> where T : new()
    {
        IEnumerable<T> GetLists(int limit = 0, long max = 0);
        Task<IEnumerable<T>> GetListAsync(int limit = 0, long max = 0);
    }
}
