using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public interface IService
    {
        T Get<T>(string uri, int limit = 0, long max = 0);
        Task<T> GetAsync<T>(string uri, int limit = 0, long max = 0);
    }
}
