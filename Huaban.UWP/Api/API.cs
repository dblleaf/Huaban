namespace Huaban.UWP
{
	using Api;
	using Models;
	public class API
	{
		public OAuthorAPI OAuthorAPI { private set; get; }
		public UserAPI UserAPI { private set; get; }
		public CategoryAPI CategoryAPI { private set; get; }
		public PinAPI PinAPI { private set; get; }
		public BoardAPI BoardAPI { private set; get; }
		private API()
		{
			OAuthorAPI = new OAuthorAPI("");
			UserAPI = new UserAPI("");
			CategoryAPI = new CategoryAPI("");
			PinAPI = new PinAPI("");
			BoardAPI = new BoardAPI("");

		}
		public void Init(string id, string secret, string callback, string md)
		{
			OAuthorAPI.setup(id, secret, callback, md);
			UserAPI.setup(id, secret, callback, md);
			CategoryAPI.setup(id, secret, callback, md);
			PinAPI.setup(id, secret, callback, md);
			BoardAPI.setup(id, secret, callback, md);
		}

		private static API _API;
		public static API Current()
		{
			if (_API == null)
			{
				_API = new API();
				_API.Init("1d912cae47144fa09d88", "f94fcc09b59b4349a148a203ab2f20c7", "huaban://android/callback", "com.huaban.android");
			}
			return _API;
		}

		public void SetToken(AuthToken token)
		{
			OAuthorAPI.setToken(token);
			UserAPI.setToken(token);
			CategoryAPI.setToken(token);
			PinAPI.setToken(token);
			BoardAPI.setToken(token);
		}
	}
}
