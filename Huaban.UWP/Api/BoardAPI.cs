using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Huaban.UWP.Api
{
	using Models;
	public class BoardAPI : APIBase
	{
		public BoardAPI(string ctx) : base(ctx) { }

		/// <summary>
		/// 添加画板
		/// </summary>
		/// <param name="boardTitle"></param>
		/// <param name="category"></param>
		/// <returns></returns>
		public async Task<Board> add(String boardTitle, String category = "")
		{
			//http://api.huaban.com/users/18116739/boards/?limit=90

			List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
			paramList.Add(new KeyValuePair<string, string>("title", boardTitle));
			if (string.IsNullOrEmpty(category))
				paramList.Add(new KeyValuePair<string, string>("credition", "false"));
			else
				paramList.Add(new KeyValuePair<string, string>("category", category));

			string json = await Post("http://api.huaban.com/boards/", paramList.ToArray());
			JObject obj = JObject.Parse(json);
			var board = Board.Parse(obj["board"] as JObject);
			return board;
		}

		/// <summary>
		/// 关注画板/取消关注
		/// </summary>
		/// <param name="boadID"></param>
		/// <param name="follow"></param>
		/// <returns></returns>
		public async Task follow(string boadID, bool follow)
		{
			string action = follow ? "follow" : "unfollow";
			string uri = $"http://api.huaban.com/boards/{boadID}/{action}/";
			await Post(uri);
		}

		public async Task<List<Pin>> GetPins(string boardID, long max = 0, int limit = 20)
		{
			string maxStr = "";
			if (max > 0)
				maxStr = "&max=" + max;
			string uri = $"http://api.huaban.com/boards/{boardID}/pins/?limit={limit}{maxStr}";

			string json = await Get(uri);
			JObject obj = JObject.Parse(json);
			return Pin.ParseList(obj["pins"] as JArray);

		}

	}
}
