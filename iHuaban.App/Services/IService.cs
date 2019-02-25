using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public interface IService<T>
    {
        T Get(string uri, int limit = 0, long max = 0);
        Task<T> GetAsync(string uri, int limit = 0, long max = 0);
    }
}
