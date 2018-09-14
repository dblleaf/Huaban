using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iHuaban.Core.Helpers
{
    public interface IHttpHelper
    {
        string Get(string url);
        Task<string> GetAsync(string url);
        TResult Get<TResult>(string url);
        Task<TResult> GetAsync<TResult>(string url);

        string Post(string url, params KeyValuePair<string, string>[] keyValues );
        Task<string> PostAsync(string url, params KeyValuePair<string, string>[] keyValues);
        TResult Post<TResult>(string url, params KeyValuePair<string, string>[] keyValues);
        Task<TResult> PostAsync<TResult>(string url, params KeyValuePair<string, string>[] keyValues);
    }
}
