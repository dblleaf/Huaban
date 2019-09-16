using iHuaban.App.Models;
using iHuaban.App.Services;
using iHuaban.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace iHuaban.App.Helpers
{
    public class AuthHttpHelper : HttpHelper, IAuthHttpHelper
    {
        public override string BaseUrl => "https://huaban.com";
        private Context context;
        private IStorageService storageService;
        public AuthHttpHelper(Context context, IStorageService storageService)
        {
            this.context = context;
            this.storageService = storageService;
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
            HttpClientHandler handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                CookieContainer = this.context.CookieContainer,
            };

            var client = new HttpClient(handler);

            return client;
        }

        protected override async Task<HttpRequestMessage> GetHttpRequestMessageAsync(HttpMethod httpMethod, Uri uri, Dictionary<string, string> headers = null, object content = null)
        {
            var request = await base.GetHttpRequestMessageAsync(httpMethod, uri, headers, content);

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            request.Headers.Add("Connection", "keep-alive");
            request.Headers.Add("Host", "huaban.com");
            request.Headers.Add("Sec-Fetch-Mode", "cors");
            request.Headers.Add("Sec-Fetch-Site", "same-origin");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3851.0 Safari/537.36 Edg/77.0.223.0");
            request.Headers.Add("X-Request", "JSON");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            if (this.context.Cookies != null && this.context.Cookies.Any())
            {
                //request.Headers.Add("Cookie", this.context.CookieString);
            }
            return request;
        }

        protected override void AfterRequest(HttpResponseMessage response)
        {
            base.AfterRequest(response);
            try
            {
                var cookies = this.context
                     .CookieContainer
                     .GetCookies(new Uri(Constants.UrlBase))
                     .Cast<Cookie>();

                storageService.SaveSetting("Cookies", JsonConvert.SerializeObject(cookies));
            }
            catch { }
        }
    }
}
