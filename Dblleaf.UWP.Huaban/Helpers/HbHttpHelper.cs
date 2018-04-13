using Dblleaf.UWP.Huaban.Models;
using Dblleaf.UWP.Huaban.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Dblleaf.UWP.Huaban.Helpers
{
    public class HbHttpHelper : HttpHelper
    {
        protected IClient Client { get; set; }
        public HbHttpHelper(IClient client)
        {
            Client = client;
        }

        public override HttpRequestMessage CreateRequest(HttpMethod method, Uri uri, params KeyValuePair<string, string>[] valueNameConnection)
        {
            var request = new HttpRequestMessage(method, uri);
            request.Headers.Add("User-Agent", "Apache-HttpClient/UNAVAILABLE (java 1.4)");
            request.Headers.Add(Constants.X_Client_ID, Client.ClientID);
            request.Headers.Add("Host", "api.huaban.com");
            request.Headers.Add(Constants.X_Client_ID, Client.ClientInfo);
            request.Headers.Add(Constants.Authorization, "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(Client.ClientInfo)));

            if (!string.IsNullOrEmpty(Client.Token?.access_token))
                request.Headers.Add(Constants.Authorization, Client.Token.token_type + " " + Client.Token.access_token);

            request.Content = new FormUrlEncodedContent(valueNameConnection);
            return request;
        }
    }
}
