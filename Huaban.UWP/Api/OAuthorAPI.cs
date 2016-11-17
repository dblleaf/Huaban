using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Huaban.UWP.Api
{
	using Models;

	public class OAuthorAPI : APIBase
	{
		public OAuthorAPI(string ctx) : base(ctx)
		{ }

		public string RequestToken(SNSType type)
		{
			String url = $"https://huaban.com/oauth/{type.strType}/?auth_callback={mOAuthCallback}&client_id={mClientID}&md={mMD}";
			return url;
		}

		public async Task<AuthToken> GetToken(string userName, string password)
		{
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Add(X_Client_ID, mClientInfo);
			client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(mClientInfo)));
			var resphonse = await
				client.PostAsync(
					"https://huaban.com/oauth/access_token/",
					new FormUrlEncodedContent(new KeyValuePair<string, string>[] {
						new KeyValuePair<string, string>("grant_type", "password"),
						new KeyValuePair<string, string>("username", userName),
						new KeyValuePair<string, string>("password", password)
				})
			);
			return AuthToken.Parse(await resphonse.Content.ReadAsStringAsync());
		}

		public async Task<AuthToken> RefreshToken(AuthToken token)
		{
			try
			{
				HttpClient client = new HttpClient();
				client.DefaultRequestHeaders.Add(X_Client_ID, mClientInfo);
				client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(mClientInfo)));
				var resphonse = await
					client.PostAsync(
						"https://huaban.com/oauth/access_token/",
						new FormUrlEncodedContent(new KeyValuePair<string, string>[] {
						new KeyValuePair<string, string>("grant_type", "refresh_token"),
						new KeyValuePair<string, string>("refresh_token", token.refresh_token)
					})
				);
				return AuthToken.Parse(await resphonse.Content.ReadAsStringAsync());
			}
			catch { }
			return null;
		}
	}
}
