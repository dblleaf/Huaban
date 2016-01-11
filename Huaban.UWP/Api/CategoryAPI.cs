using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Huaban.UWP.Api
{
	using Models;
	public class CategoryAPI : APIBase
	{
		//首页
		//http://api.huaban.com/all/?limit=20&max=545163525
		//http://api.huaban.com/all/?limit=20

		//分类
		//http://api.huaban.com/favorite/desire?limit=20
		//http://api.huaban.com/favorite/desire?limit=20&max=541614393

		//最热
		//http://api.huaban.com/popular/?limit=20
		//http://api.huaban.com/popular/?limit=20&max=545111576

		//搜索
		//http://api.huaban.com/search/boards/?page=1&per_page=20&q=%E7%8B%97
		//http://api.huaban.com/search/?page=1&per_page=20&q=%E7%8B%97
		//http://api.huaban.com/search/?q=%E7%8B%97
		public CategoryAPI(string ctx) : base(ctx) { }
		public async Task<List<Category>> GetCategoryList()
		{

			return await Get(
				"http://api.huaban.com/categories/",
				o =>
				{
					List<Category> list = new List<Category>();


					var obj = JObject.Parse(o);
					var array = obj["categories"] as JArray;
					foreach (JObject jobj in array)
					{
						list.Add(new Category()
						{
							name = jobj["name"].ToString(),
							nav_link = jobj["nav_link"].ToString(),
							id = jobj["id"].ToString(),
							urlname = jobj["urlname"].ToString()
						});
					}
					return list;
				}
			);
		}

		public async Task<List<Pin>> GetCategoryPinList(string nav_link, int limit = 20, long max = 0)
		{
			string json = await GetCategoryPinStrng(nav_link, limit, max);
			var obj = JObject.Parse(json);
			return Pin.ParseList(obj["pins"] as JArray);
		}

		public async Task<string> GetCategoryPinStrng(string nav_link, int limit = 20, long max = 0)
		{
			string maxStr = max.ToString();
			if (max <= 0)
				maxStr = "";
			nav_link = nav_link.TrimStart('/');
			return await Get($"http://api.huaban.com/{nav_link}?limit={limit}&max={maxStr}");
		}
	}
}
