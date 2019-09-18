using iHuaban.App.Models;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(string userName, string password);
        Task LoadMeAsync();
    }
}
