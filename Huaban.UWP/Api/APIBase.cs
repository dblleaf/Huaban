using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Huaban.UWP.Api
{
	using Models;
	public abstract class APIBase : HttpHelper
	{
		protected const String API_ACTIVITIES = "http://api.huaban.com/message/activities/";
		protected const String API_ALL = "http://api.huaban.com/all/";
		protected const String API_BOARDS = "http://api.huaban.com/boards/";
		protected const String API_CATEGORY = "http://api.huaban.com/categories/";
		protected const String API_FAVORITE = "http://api.huaban.com/favorite/";
		protected const String API_FEEDS = "http://api.huaban.com/feeds/";
		protected const String API_FRIENDS = "http://api.huaban.com/friends/";
		protected const String API_Follow = "http://api.huaban.com/following/";
		protected const String API_HOST = "https://huaban.com/";
		protected const String API_MAIN = "http://api.huaban.com/";
		protected const String API_MENTIONS = "http://api.huaban.com/message/mentions/";
		protected const String API_MESSAGE = "http://api.huaban.com/message/";
		protected const String API_PIN = "http://api.huaban.com/pins/";
		protected const String API_POPULAR_PINS = "http://api.huaban.com/popular/";
		protected const String API_REPORT = "http://api.huaban.com/feedback/report/";
		protected const String API_SEARCH_BOARD = "http://api.huaban.com/search/boards/";
		protected const String API_SEARCH_PEOPLE = "http://api.huaban.com/search/people/";
		protected const String API_SEARCH_PIN = "http://api.huaban.com/search/";
		protected const String API_SHARE = "http://api.huaban.com/share/";
		protected const String API_TOKEN = "https://huaban.com/oauth/access_token/";
		protected const String API_USER = "http://api.huaban.com/users/";
		protected const String API_WEEKLY = "http://api.huaban.com/weekly/";
		private const String Authorization = "Authorization";
		private const int CONNECTION_TIMEOUT = 0x4e20;
		private const Boolean DEBUG = true;
		public const String HBIMG = "http://img.hb.aicdn.com/";
		protected const String LIMIT = "limit";
		protected const String MAX = "max";
		protected const String SINCE = "since";
		private const int SO_TIMEOUT = 0x4e20;
		protected const String X_Client_ID = "X-Client-ID";

		protected String mClientID;
		protected String mClientInfo;
		protected String mClientSecret;

		protected String mMD;
		protected String mOAuthCallback;
		protected AuthToken mToken;

		private string _ctx;
		public APIBase(string ctx)
		{
			_ctx = ctx;
		}

		public void setup(String mclientID, String mclientSecret, String callback, String md)
		{
			mClientID = mclientID;
			mClientSecret = mclientSecret;
			mClientInfo = mClientID + ":" + mClientSecret;
			mOAuthCallback = callback;
			mMD = md;
		}

		public void setToken(AuthToken token)
		{
			mToken = token;
		}


		public override HttpRequestMessage CreateRequest(HttpMethod method, Uri uri, params KeyValuePair<string, string>[] valueNameConnection)
		{
			var request = new HttpRequestMessage(method, uri);
			request.Headers.Add("User-Agent", "Apache-HttpClient/UNAVAILABLE (java 1.4)");
			request.Headers.Add(X_Client_ID, mClientInfo);
			request.Headers.Add("Host", "api.huaban.com");
			if (mToken != null)
				request.Headers.Add(Authorization, mToken.token_type + " " + mToken.access_token);
			request.Content = new FormUrlEncodedContent(valueNameConnection);
			return request;
		}
	}
}
