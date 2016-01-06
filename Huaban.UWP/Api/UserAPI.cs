using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Huaban.UWP.Api
{
	using Models;

	public class UserAPI : APIBase
	{
		private const string API_SIGNUP = "http://api.huaban.com/signup/";

		public UserAPI(string ctx) : base(ctx) { }

		public async Task<User> GetSelf()
		{
			return await Get("http://api.huaban.com/users/me", o => SerializeExtension.JsonDeserlialize<User>(o));
		}
		//某人的画板
		public async Task<List<Board>> GetBoards(string userID, long max)
		{
			string maxBoardID = "&max=" + max.ToString();
			if (max <= 0)
				maxBoardID = "";
			string uri = $"http://api.huaban.com/users/{userID}/boards/?limit=20{maxBoardID}";
			string json = await Get(uri);

			var obj = JObject.Parse(json);
			var list = Board.ParseList(obj["boards"] as JArray);
			return list;
		}

		//关注的采集
		public async Task<List<Pin>> GetFollowing(long max)
		{
			string maxPin = "";
			if (max > 0)
				maxPin = "&max=" + max;
			string uri = $"http://api.huaban.com/following?limit=20{maxPin}";
			string json = await Get(uri);
			JObject obj = JObject.Parse(json);
			var list = Pin.ParseList(obj["pins"] as JArray);
			return list;
		}
		//某人的采集
		public async Task<List<Pin>> GetPins(string userID, long max)
		{
			string maxPin = "";
			if (max > 0)
				maxPin = "&max=" + max;

			string uri = $"http://api.huaban.com/users/{userID}/pins/?limit=20{maxPin}";
			string json = await Get(uri);
			JObject obj = JObject.Parse(json);
			var list = Pin.ParseList(obj["pins"] as JArray);
			return list;
		}
		//某人喜欢的采集
		public async Task<List<Pin>> GetLikePins(string userID, long max)
		{
			string maxPin = "";
			if (max > 0)
				maxPin = "&max=" + max;

			string uri = $"http://api.huaban.com/users/{userID}/likes/?limit=20{maxPin}";
			string json = await Get(uri);
			JObject obj = JObject.Parse(json);
			var list = Pin.ParseList(obj["pins"] as JArray);
			return list;
		}
		//某人关注的画板
		public async Task<List<Board>> GetFollowingBoards(string userID, int page = 0)
		{
			if (page <= 0)
				page = 1;
			string uri = $"http://api.huaban.com/{userID}/following/boards/?page={page}&per_page=20&wfl=1";
			string json = await Get(uri);
			JObject obj = JObject.Parse(json);
			var list = Board.ParseList(obj["boards"] as JArray);
			return list;
		}


	}
}
