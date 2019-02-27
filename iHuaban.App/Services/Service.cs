using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public abstract class Service<T> : IService<T>
    {
        protected IServiceProvider ServiceProvider { get; private set; }
        public Service(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public T Get(string uri, int limit = 0, long max = 0)
        {
           return this.ServiceProvider.Get<T>(uri, limit, max);
        }

        public async Task<T> GetAsync(string uri, int limit = 0, long max = 0)
        {
            return await this.ServiceProvider.GetAsync<T>(uri, limit, max);
        }
    }
}
