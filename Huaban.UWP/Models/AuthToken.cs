using System;

namespace Huaban.UWP.Models
{
	using UWP;
	public class AuthToken
	{
		public String access_token;
		public int expires_in;
		public String refresh_token;

		public String token_type;
		public DateTime ExpiresIn { set; get; }
		public static AuthToken Parse(string text)
		{
			var token = SerializeExtension.JsonDeserlialize<AuthToken>(text);
			token.ExpiresIn = DateTime.Now.AddSeconds(token.expires_in);
			return token;
		}
	}
}
