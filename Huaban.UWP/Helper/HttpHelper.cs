using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;

namespace Huaban.UWP
{
	public class HttpHelper
	{
		public static HttpHelper Factory
		{
			get
			{
				return new HttpHelper();
			}
		}

		public HttpRequestMessage CreateRequest(HttpMethod method, string uri, params KeyValuePair<string, string>[] valueNameConnection)
		{
			return CreateRequest(method, new Uri(uri), valueNameConnection);
		}

		public virtual HttpRequestMessage CreateRequest(HttpMethod method, Uri uri, params KeyValuePair<string, string>[] valueNameConnection)
		{
			var request = new HttpRequestMessage(method, uri);
			request.Content = new FormUrlEncodedContent(valueNameConnection);
			return request;
		}

		public async Task<string> Get(HttpRequestMessage requestMessage)
		{
			requestMessage.Method = HttpMethod.Get;
			return await Request(requestMessage);
		}
		public async Task<T> Get<T>(HttpRequestMessage requestMessage, Func<string, T> func)
		{
			var result = await Get(requestMessage);
			return func(result);
		}

		public async Task<string> Get(string uri)
		{
			HttpRequestMessage requestMessage = CreateRequest(HttpMethod.Get, new Uri(uri));
			return await Get(requestMessage);
		}

		public async Task<T> Get<T>(string uri, Func<string, T> func)
		{
			var result = await Get(uri);
			return func(result);
		}

		public async Task<string> Post(HttpRequestMessage requestMessage)
		{
			requestMessage.Method = HttpMethod.Post;

			return await Request(requestMessage);
		}

		public async Task<T> Post<T>(HttpRequestMessage requestMessage, Func<string, T> func)
		{
			var result = await Post(requestMessage);
			return func(result);
		}

		public async Task<string> Post(string uri, params KeyValuePair<string, string>[] valueNameConnection)
		{
			HttpRequestMessage requestMessage = CreateRequest(HttpMethod.Post, uri, valueNameConnection);
			return await Post(requestMessage);
		}

		public async Task<T> Post<T>(string uri, Func<string, T> func, params KeyValuePair<string, string>[] valueNameConnection)
		{
			HttpRequestMessage requestMessage = CreateRequest(HttpMethod.Post, new Uri(uri), valueNameConnection);
			var result = await Post(uri);
			return func(result);
		}

		public async Task<string> Request(HttpRequestMessage requestMessage)
		{
			HttpClient client = DefaultHttpClient();
			HttpResponseMessage response = null;

			response = await client.SendAsync(requestMessage);

			var result = await response.Content.ReadAsStringAsync();
			return result;
		}

		public virtual HttpClient DefaultHttpClient()
		{
			HttpClient client = new HttpClient();
			return client;
		}

		public async Task<byte[]> GetBytes(string uri)
		{
			HttpClient client = new HttpClient();
			return await client.GetByteArrayAsync(uri);
		}

	}
}
