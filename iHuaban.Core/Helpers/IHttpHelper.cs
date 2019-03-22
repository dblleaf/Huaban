using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace iHuaban.Core.Helpers
{
    public interface IHttpHelper
    {
        Task<string> GetStringAsync(string url, Dictionary<string, string> headers = null);
        Task<T> GetAsync<T>(string url, Dictionary<string, string> headers = null);
        Task<string> PostAsync(string url, Dictionary<string, string> headers = null, object content = null);
        Task<T> PostAsync<T>(string url, Dictionary<string, string> headers = null, object content = null);
        Task<T> SendRequestAsync<T>(HttpMethod httpMethod, string url, Dictionary<string, string> headers = null, object content = null);
        Task<string> SendRequestStringAsync(HttpMethod httpMethod, string url, Dictionary<string, string> headers = null, object content = null);
    }
}
