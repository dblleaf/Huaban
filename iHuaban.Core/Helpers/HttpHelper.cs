using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace iHuaban.Core.Helpers
{
    public class HttpHelper : IHttpHelper
    {
        public Action<HttpRequestMessage> OnCreateHttpRequestMessage;
        public Action<HttpClient> OnCreateHttpClient;
        protected virtual HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            OnCreateHttpClient?.Invoke(client);
            return client;
        }

        protected virtual TResult DeserializeObject<TResult>(string content)
        {
            return JsonConvert.DeserializeObject<TResult>(content);
        }

        public string Get(string url)
        {
            return GetAsync(url).Result;
        }

        public TResult Get<TResult>(string url)
        {
            return GetAsync<TResult>(url).Result;
        }

        public async Task<string> GetAsync(string url)
        {
            return await GetClient().GetStringAsync(url);
        }

        public async Task<TResult> GetAsync<TResult>(string url)
        {
            var content = await GetAsync(url);
            return DeserializeObject<TResult>(content);
        }

        public string Post(string url, params KeyValuePair<string, string>[] keyValues)
        {
            return PostAsync(url, keyValues).Result;
        }

        public TResult Post<TResult>(string url, params KeyValuePair<string, string>[] keyValues)
        {
            var content = PostAsync(url, keyValues).Result;
            return DeserializeObject<TResult>(content);
        }

        public async Task<string> PostAsync(string url, params KeyValuePair<string, string>[] keyValues)
        {
            HttpContent httpContent = null;
            if (keyValues?.Length > 0)
            {
                httpContent = new FormUrlEncodedContent(keyValues);
            }
            var httpResponse = await GetClient().PostAsync(url, httpContent);
            return await httpResponse.Content.ReadAsStringAsync();
        }

        public async Task<TResult> PostAsync<TResult>(string url, params KeyValuePair<string, string>[] keyValues)
        {
            var content = await PostAsync(url, keyValues);
            return DeserializeObject<TResult>(content);
        }
    }
}
