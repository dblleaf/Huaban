using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

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
		public async Task<List<Board>> GetRelatedBoards(string PinID, int max)
		{

			string maxBoardID = "&max=" + max.ToString();
			if (max <= 0)
				maxBoardID = "";
			string uri = $"http://api.huaban.com/pins/{PinID}/relatedboards/?limit=12{maxBoardID}";
			string json = await Get(uri + maxBoardID);

			var obj = JObject.Parse(json);
			var list = Board.ParseList(obj["boards"] as JArray);
			return list;
		}
	}
}
