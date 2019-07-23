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
    public class HbHttpHelper : HttpHelper
    {
        private Context context;
        public HbHttpHelper(Context context)
        {
            this.context = context;
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
            var handler = new HttpClientHandler
            {
                CookieContainer = this.context.Cookies,
                UseCookies = true,
                UseDefaultCredentials = false
            };

            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            client.DefaultRequestHeaders.Add("Host", "huaban.com");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3843.0 Safari/537.36 Edg/77.0.218.4");
            client.DefaultRequestHeaders.Add("X-Request", "JSON");
            client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

            return client;
        }

    }
}
