using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Huaban.UWP.Services
{
    using Models;

    public class OAuthorService : ServiceBase
    {
        public OAuthorService(IClient client)
            : base(client) { }

        public string RequestToken(SNSType type)
        {
            String url = $"https://huaban.com/oauth/{type.strType}/?auth_callback={Client.OAuthCallback}&client_id={Client.ClientID}&md={Client.MD}";
            return url;
        }

        public override HttpRequestMessage CreateRequest(HttpMethod method, Uri uri, params KeyValuePair<string, string>[] valueNameConnection)
        {
            var request = base.CreateRequest(method, uri, valueNameConnection);
            request.Headers.Add(Constants.X_Client_ID, Client.ClientInfo);
            request.Headers.Add("host", "huaban.com");

            if (!request.Headers.Contains(Constants.Authorization))
            {
                request.Headers.Add(Constants.Authorization, "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(Client.ClientInfo)));
            }
            else
            {
                var aaa = Convert.ToBase64String(Encoding.UTF8.GetBytes(Client.ClientInfo));

                //request.Headers.Add(Constants.Authorization, "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(Client.ClientInfo)));
            }
            return request;
        }
        public async Task<AuthToken> GetToken(string userName, string password)
        {
            var resphonse = await
                Post(
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
                    Post(
                        Constants.API_TOKEN,
                        new KeyValuePair<string, string>("grant_type", "refresh_token"),
                        new KeyValuePair<string, string>("refresh_token", token.access_token)
                );
                return AuthToken.Parse(resphonse);
            }
            catch (Exception ex)
            {
                var aaa = ex;
            }
            return null;
        }
    }
}
