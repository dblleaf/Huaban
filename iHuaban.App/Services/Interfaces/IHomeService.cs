using iHuaban.App.Models;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public interface IHomeService
    {
        Home GetPagingHome(int page);
        Task<Home> GetPagingHomeAsync(int page);
    }
}
