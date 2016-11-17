using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static System.Net.WebUtility;

namespace Huaban.UWP.Api
{
	using Models;
	public class PinAPI : APIBase
	{
		public PinAPI(string ctx) : base(ctx) { }

		public async Task<Pin> GetPin(string PinID)
		{
			string uri = $"http://api.huaban.com/pins/{PinID}/";
			string json = await Get(uri);
			var obj = JObject.Parse(json);
			return Models.Pin.Parse(obj["pin"] as JObject, true);
		}
		/// <summary>
		/// 喜欢/取消喜欢
		/// </summary>
		/// <param name="PinID"></param>
		/// <param name="liked"></param>
		/// <returns></returns>
		public async Task<string> Like(string PinID, bool liked)
		{
			string action = liked ? "like" : "unlike";
			string uri = $"http://api.huaban.com/pins/{PinID}/{action}/";
			return await Post(uri);
		}

		/// <summary>
		/// 采集
		/// </summary>
		/// <returns></returns>
		public async Task<Models.Pin> Pin(string PinID, string BoardID, string text = "")
		{
			string uri = "http://api.huaban.com/pins/";
			var json = await Post(uri,
						new KeyValuePair<string, string>("board_id", BoardID),
						new KeyValuePair<string, string>("text", text),
						new KeyValuePair<string, string>("via", PinID),
						new KeyValuePair<string, string>("share_button", "0"));
			var obj = JObject.Parse(json);
			var pin = Models.Pin.Parse(obj["pin"] as JObject);
			return pin;
		}

		//http://api.huaban.com/pins/552897183/relatedboards/?max=38
		//转采的画板（cover是封面）
		public async Task<List<Board>> GetRelatedBoards(string PinID, long max)
		{
			string maxBoardID = "&max=" + max.ToString();
			if (max <= 0)
				maxBoardID = "";
			string uri = $"http://api.huaban.com/pins/{PinID}/relatedboards/?limit=12{maxBoardID}";
			string json = await Get(uri);
			if (json == "[]")
				return new List<Board>();

			var obj = JObject.Parse(json);
			var list = Board.ParseList(obj["boards"] as JArray);
			return list;
		}

		public async Task<List<Pin>> Search(string keyword, int page = 1, int per_page = 20)
		{
			string uri = $"http://api.huaban.com/search/?q={UrlEncode(keyword)}&ijlhlz49&page={page}&per_page={per_page}&wfl=1";

			string json = await Get(uri);
			var obj = JObject.Parse(json);
			return Models.Pin.ParseList(obj["pins"] as JArray);
		}

		public async Task<List<User>> GetLikeList(string PinID, long max = 0, int limit = 20)
		{
			string maxStr = "&max=" + max.ToString();
			if (max <= 0)
				maxStr = "";
			string uri = $"http://api.huaban.com/pins/{PinID}/likes/?limit={limit}{maxStr}";
			string json = await Get(uri);

			var obj = JObject.Parse(json);
			var list = User.ParseList(obj["users"] as JArray);
			return list;
		}

		//推荐的采集
		public async Task<List<Pin>> GetRecommendList(string PinID, int page = 1, int per_page = 10)
		{
			string uri = $"http://api.huaban.com/pins/{PinID}/recommend/?ijzu3ifx&page={page}&per_page={per_page}&wfl=1";

			string json = await Get(uri);
			var arr = JArray.Parse(json);
			return Models.Pin.ParseList(arr);
		}
	}
}
