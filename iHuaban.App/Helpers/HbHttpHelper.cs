using iHuaban.App.Models;
using iHuaban.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace iHuaban.App.Helpers
{
    public class HbHttpHelper : HttpHelper, IHbHttpHelper
    {
        private Context context;
        public HbHttpHelper(Context context)
        {
            this.context = context;
        }

        public async Task<AuthToken> RefreshToken(string refreshToken)
        {
            if (context == null)
            {
                return null;
            }

            return await this.PostAsync<AuthToken>(
                Constants.ApiAccessToken,
                null,
                new RefreshTokenParameter
                {
                    refresh_token = context.AuthToken?.access_token,
                    grant_type = "refresh_token"
                }
            );
        }

        protected override void SaveResponse(HttpResponseMessage httpResponse)
        {
            if (httpResponse.Headers.Contains("Set-Cookie"))
            {
                context.Cookie = string.Join(";", httpResponse.Headers.GetValues("Set-Cookie"));
            }
        }

        protected override HttpContent GetHttpContent(object content)
        {
            if (content is IEnumerable<KeyValuePair<string, string>> dic)
            {
                return new FormUrlEncodedContent(dic);
            }

            return base.GetHttpContent(content);
        }
        protected override HttpClient GetHttpClient()
        {
            var client = base.GetHttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Huaban-iPhone-Lily/4.3.4 (iPhone; iOS 12.1.4; Scale/2.00)");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Accept-Language", "zh-Hans-CN;q=1");
            client.DefaultRequestHeaders.Add("X-Request", "JSON");
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Constants.ClientId}:{ConvertDateTimeToInt(DateTime.UtcNow)}:{Constants.ClientSecret}"));
            client.DefaultRequestHeaders.Add("client", "Basic " + token);
            client.DefaultRequestHeaders.Add("Host", "api.huaban.com");
            if (!string.IsNullOrWhiteSpace(context.Sid))
            {
                client.DefaultRequestHeaders.Add("Cookie", context.Sid);
            }
            token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Constants.ClientId}:{ConvertDateTimeToInt(DateTime.UtcNow)}:{Constants.AuthSecret}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);
            return client;
        }

        protected async override Task<HttpRequestMessage> GetHttpRequestMessageAsync(HttpMethod httpMethod, Uri uri, Dictionary<string, string> headers = null, object content = null)
        {
            var requestMessage = await base.GetHttpRequestMessageAsync(httpMethod, uri, headers, content);

            if (!string.IsNullOrEmpty(context.AuthToken?.access_token))
            {
                if (context.AuthToken.ExpiresIn <= DateTime.Now)
                {
                    var newToken = await this.RefreshToken(context.AuthToken.refresh_token);
                    if (newToken != null)
                    {
                        context.AuthToken = newToken;
                    }
                }

                if (context.AuthToken.ExpiresIn > DateTime.Now)
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue(context.AuthToken.token_type, context.AuthToken.access_token);
                }
            }

            return requestMessage;
        }

        private long ConvertDateTimeToInt(DateTime dateTime)
        {
            var startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long t = (dateTime.Ticks - startTime.Ticks) / (10000 * 1000);
            return t;
        }
    }
}
