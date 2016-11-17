using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Huaban.UWP.Api
{
    using Models;
    public abstract class APIBase : HttpHelper
    {
        protected AuthToken mToken { get; set; }
        protected IClient Client { get; set; }
        public APIBase(IClient client, AuthToken token)
        {
            Client = client;
            mToken = token;
        }

        public override HttpRequestMessage CreateRequest(HttpMethod method, Uri uri, params KeyValuePair<string, string>[] valueNameConnection)
        {
            var request = new HttpRequestMessage(method, uri);
            request.Headers.Add("User-Agent", "Apache-HttpClient/UNAVAILABLE (java 1.4)");
            request.Headers.Add(Client.X_Client_ID, Client.ClientID);
            request.Headers.Add("Host", "api.huaban.com");
            if (mToken != null && !string.IsNullOrEmpty(mToken.access_token))
                request.Headers.Add(Client.Authorization, mToken.token_type + " " + mToken.access_token);
            request.Content = new FormUrlEncodedContent(valueNameConnection);
            return request;
        }
    }
}
