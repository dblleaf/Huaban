using System.Collections.Generic;
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
		public async Task<User> GetUser(string userID)
		{
			string uri = $"http://api.huaban.com/users/{userID}";
			string json = await Get(uri);
			var obj = JObject.Parse(json);
			var user = User.Parse(obj);
			return user;
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
		public async Task<List<Board>> GetFollowingBoardList(string userID, int page = 0)
		{
			if (page <= 0)
				page = 1;
			string uri = $"http://api.huaban.com/{userID}/following/boards/?page={page}&per_page=20&wfl=1";
			string json = await Get(uri);
			JObject obj = JObject.Parse(json);
			var list = Board.ParseList(obj["boards"] as JArray);
			return list;
		}

		//某人的粉丝
		public async Task<List<User>> GetFollowerList(string userID, long max, int limit = 20)
		{
			string maxUser = "";
			if (max > 0)
				maxUser = "&max=" + max;

			string uri = $"http://api.huaban.com/users/{userID}/followers/?limit={limit}{maxUser}&wfl=1";
			string json = await Get(uri);
			JObject obj = JObject.Parse(json);
			var list = User.ParseList(obj["users"] as JArray);
			return list;
		}

		//某人关注的人
		public async Task<List<User>> GetFollowingUserList(string userID, long max, int limit = 20)
		{
			string maxUser = "";
			if (max > 0)
				maxUser = "&max=" + max;

			string uri = $"http://api.huaban.com/users/{userID}/following/?limit={limit}{maxUser}&wfl=1";
			string json = await Get(uri);
			JObject obj = JObject.Parse(json);
			var list = User.ParseList(obj["users"] as JArray);
			return list;
		}

		public async Task<string> follow(string userID, bool follow)
		{
			string action = follow ? "follow" : "unfollow";
			string uri = $"http://api.huaban.com/users/{userID}/{action}/";
			return await Post(uri);
		}
	}
}
