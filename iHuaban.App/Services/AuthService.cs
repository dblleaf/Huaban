using iHuaban.App.Helpers;
using iHuaban.App.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace iHuaban.App.Services
{
    public class AuthService : IAuthService
    {
        private IHbHttpHelper hbHttpHelper;
        public AuthService(IHbHttpHelper hbHttpHelper)
        {
            this.hbHttpHelper = hbHttpHelper;
        }

        public async Task<AuthToken> AccessTokenAsync(string userName, string password)
        {
            return await hbHttpHelper.PostAsync<AuthToken>(
                Constants.ApiAccessToken,
                content: new Dictionary<string, string>
                {
                    { "grant_type", "password"},
                    { "password", password },
                    { "username", userName },
                });
        }

        public async Task<AuthToken> RefreshTokenAsync(string refreshToken)
        {
            return await hbHttpHelper.RefreshToken(refreshToken);
        }
    }
}
