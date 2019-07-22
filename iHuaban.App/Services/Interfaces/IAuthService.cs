using iHuaban.App.Models;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string userName, string password);
    }
}
