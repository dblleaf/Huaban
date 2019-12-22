using Huaban.UWP.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Huaban.UWP.Services
{
    public abstract class ServiceBase: HttpHelper
    {
        protected IClient Client { get; set; }
        public ServiceBase(IClient client)
        {
            Client = client;
        }

        public override HttpRequestMessage CreateRequest(HttpMethod method, Uri uri, params KeyValuePair<string, string>[] valueNameConnection)
        {
            var request = new HttpRequestMessage(method, uri);
            request.Headers.Add("User-Agent", "Apache-HttpClient/UNAVAILABLE (java 1.4)");

            if (!string.IsNullOrEmpty(Client.Token?.access_token))
                request.Headers.Add(Constants.Authorization, Client.Token.token_type + " " + Client.Token.access_token);

            request.Content = new FormUrlEncodedContent(valueNameConnection);
            return request;
        }
    }
}
