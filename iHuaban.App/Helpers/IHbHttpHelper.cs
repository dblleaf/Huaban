using iHuaban.App.Models;
using iHuaban.Core.Helpers;
using System.Threading.Tasks;

namespace iHuaban.App.Helpers
{
    public interface IHbHttpHelper : IHttpHelper
    {
        Task<AuthToken> RefreshToken(string refreshToken);
    }
}
