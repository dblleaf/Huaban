using iHuaban.App.Models;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public interface IAuthService
    {
        Task<AuthToken> AccessTokenAsync(string userName, string password);
        Task<AuthToken> RefreshTokenAsync(string refreshToken);
    }
}
