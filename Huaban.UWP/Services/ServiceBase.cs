using Huaban.UWP.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Huaban.UWP.Services
{
    public abstract class ServiceBase : HttpHelper
    {
        protected IClient Client { get; set; }
        public ServiceBase(IClient client)
        {
            Client = client;
            this.AfterRequest += this.SaveSid;
        }

        public override HttpRequestMessage CreateRequest(HttpMethod method, Uri uri, params KeyValuePair<string, string>[] valueNameConnection)
        {
            var request = new HttpRequestMessage(method, uri);

            request.Headers.Add("User-Agent", "Huaban-iPhone-Lily/4.4.10 (iPhone; iOS 13.3; Scale/2.00)");

            if (!string.IsNullOrEmpty(Client.Token?.access_token))
                request.Headers.Add(Constants.Authorization, Client.Token.token_type + " " + Client.Token.access_token);

            request.Content = new FormUrlEncodedContent(valueNameConnection);

            return request;
        }

        private void SaveSid(HttpResponseMessage responseMessage)
        {
            var aaa = responseMessage;
        }
    }
}
