using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace iHuaban.Core.Helpers
{
    public class HttpHelper : IHttpHelper
    {
        protected virtual async Task<HttpRequestMessage> GetHttpRequestMessageAsync(HttpMethod httpMethod, Uri uri, Dictionary<string, string> headers = null, object content = null)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod, uri);

            if (headers?.Count > 0)
            {
                foreach (var keyValue in headers)
                {
                    if (string.IsNullOrWhiteSpace(keyValue.Key) || string.IsNullOrWhiteSpace(keyValue.Value))
                    {
                        continue;
                    }
                    httpRequestMessage.Headers.Add(keyValue.Key, keyValue.Value);
                }
            }

            if (content != null)
            {
                httpRequestMessage.Content = GetHttpContent(content);
            }

            return await Task.FromResult(httpRequestMessage);
        }

        protected virtual HttpContent GetHttpContent(object content)
        {
            return new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        }

        protected virtual HttpClient GetHttpClient()
        {
            return new HttpClient();
        }

        public async Task<string> GetStringAsync(string url, Dictionary<string, string> headers = null)
        {
            return await SendRequestStringAsync(HttpMethod.Get, url, headers, null);
        }

        public async Task<T> GetAsync<T>(string url, Dictionary<string, string> headers = null)
        {
            return await SendRequestAsync<T>(HttpMethod.Get, url, headers);
        }

        public async Task<string> PostAsync(string url, Dictionary<string, string> headers = null, object content = null)
        {
            return await SendRequestStringAsync(HttpMethod.Post, url, headers, content);
        }

        public async Task<T> PostAsync<T>(string url, Dictionary<string, string> headers = null, object content = null)
        {
            return await SendRequestAsync<T>(HttpMethod.Post, url, headers, content);
        }

        public async Task<T> SendRequestAsync<T>(HttpMethod httpMethod, string url, Dictionary<string, string> headers = null, object content = null)
        {
            var json = await SendRequestStringAsync(httpMethod, url, headers, content);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<string> SendRequestStringAsync(HttpMethod httpMethod, string url, Dictionary<string, string> headers = null, object content = null)
        {
            using (var client = GetHttpClient())
            {
                var requestMessage = await GetHttpRequestMessageAsync(httpMethod, new Uri(url), headers, content);
                var response = await client.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
        }
    }
}
