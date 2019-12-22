using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Huaban.UWP.Services
{
    using Models;
    using System.Linq;

    public class OAuthorService : ServiceBase
    {
        public OAuthorService(IClient client)
            : base(client) { }

        public override HttpRequestMessage CreateRequest(HttpMethod method, Uri uri, params KeyValuePair<string, string>[] valueNameConnection)
        {
            var aa = "";
            aa.Reverse().ToString();
            var request = base.CreateRequest(method, uri, valueNameConnection);

            if (!request.Headers.Contains(Constants.Authorization))
            {
                var timestamp = this.GetTimeStamp();
                string base64 = (Constants.ClentId.ReverseString() + ":" + timestamp + ":" + (Constants.ClientSecret.ReverseString() + timestamp).ToMd5()).ToBase64();
                request.Headers.Add(Constants.Authorization, "Basic " + base64);
                request.Headers.Add("client", "Basic " + base64);
                request.Headers.Add("Host", "api.huaban.com");
                request.Headers.Add("X-Request", "JSON");
                request.Headers.Add("Accept", "application/json");
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

        private string GetTimeStamp()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }
}
