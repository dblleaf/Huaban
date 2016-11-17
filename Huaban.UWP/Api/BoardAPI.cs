using System;
using System.Collections.Generic;
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
		/// 编辑画板
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		public async Task<Board> edit(Board board)
		{
			List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
			paramList.Add(new KeyValuePair<string, string>("title", board.title));
			paramList.Add(new KeyValuePair<string, string>("description", board.description));
			paramList.Add(new KeyValuePair<string, string>("category", board.category_id));
			string json = await Post($"http://api.huaban.com/boards/{board.board_id}", paramList.ToArray());
			return board;
		}

		/// <summary>
		/// 删除画板
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		public async Task delete(Board board)
		{
			List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
			paramList.Add(new KeyValuePair<string, string>("_method", "DELETE"));
			
			string json = await Post($"http://api.huaban.com/boards/{board.board_id}", paramList.ToArray());
		}

		/// <summary>
		/// 关注画板/取消关注
		/// </summary>
		/// <param name="boadID"></param>
		/// <param name="follow"></param>
		/// <returns></returns>
		public async Task<string> follow(string boadID, bool follow)
		{
			string action = follow ? "follow" : "unfollow";
			string uri = $"http://api.huaban.com/boards/{boadID}/{action}/";
			return await Post(uri);
		}

		public async Task<List<Pin>> GetPins(string boardID, long max = 0, int limit = 20)
		{
			string maxStr = "";
			if (max > 0)
				maxStr = "&max=" + max;
			string uri = $"http://api.huaban.com/boards/{boardID}/pins/?limit={limit}{maxStr}";

			string json = await Get(uri);
			JObject obj = JObject.Parse(json);
			return Pin.ParseList(obj["pins"] as JArray, true);

		}

		public async Task<Board> GetPinsWithBoard(string boardID, long max = 0, int limit = 20)
		{
			string maxStr = "";
			if (max > 0)
				maxStr = "&max=" + max;
			string uri = $"http://api.huaban.com/boards/{boardID}/?iiyjjrxz&limit={limit}{maxStr}&wfl=1";

			string json = await Get(uri);
			JObject obj = JObject.Parse(json);
			var objBoard = obj["board"] as JObject;
			var board = Board.Parse(obj["board"] as JObject);

			return board;
		}

		public async Task<Board> GetBoard(string boardID)
		{

			string uri = $"http://api.huaban.com/boards/{boardID}";

			string json = await Get(uri);
			JObject obj = JObject.Parse(json);
			var objBoard = obj["board"] as JObject;
			var board = Board.Parse(obj["board"] as JObject);

			return board;
		}
	}
}
