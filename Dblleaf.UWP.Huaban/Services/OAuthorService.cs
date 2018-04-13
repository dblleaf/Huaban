using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dblleaf.UWP.Huaban.Services
{
    using Dblleaf.UWP.Huaban.Helpers;
    using Models;

    public class OAuthorService : ServiceBase
    {
        public OAuthorService(HbHttpHelper httpHelper)
            : base(httpHelper) { }

        public async Task<AuthToken> GetToken(string userName, string password)
        {
            var resphonse = await
                HttpHelper.Post(
                    Constants.API_TOKEN,
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", userName),
                    new KeyValuePair<string, string>("password", password)
                );
            return AuthToken.Parse(resphonse);
        }

        public async Task<AuthToken> RefreshToken(AuthToken token)
        {
            try
            {
                var resphonse = await
                    HttpHelper.Post(
                        Constants.API_TOKEN,
                        new KeyValuePair<string, string>("grant_type", "refresh_token"),
                        new KeyValuePair<string, string>("refresh_token", token.refresh_token)
                );
                return AuthToken.Parse(resphonse);
            }
            catch { }
            return null;
        }
    }
}
