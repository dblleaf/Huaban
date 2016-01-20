using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Huaban.UWP.Models
{
	using Base;
	public class User : ObservableObject
	{
		public ImageFile avatar { set; get; }
		public int board_count { set; get; }
		public List<Board> boards { set; get; }
		public String created_at { set; get; }
		public String email { set; get; }
		public int follower_count { set; get; }
		public List<User> followers { set; get; }

		private bool _following;
		public bool following
		{
			get { return _following; }
			set { SetValue(ref _following, value); }
		}
		public int following_count { set; get; }
		public List<User> followings { set; get; }
		public int like_count { set; get; }
		public List<Pin> likes { set; get; }
		public Bindings mBindings { set; get; }
		public int pin_count { set; get; }
		public List<Pin> pins { set; get; }
		public Profile profile { set; get; }
		public int seq { set; get; }

		public String urlname { set; get; }
		public String user_id { set; get; }
		public String username { set; get; }

		public static User Parse(string json)
		{
			JObject obj = JObject.Parse(json);
			return User.Parse(json);
		}
		public static User Parse(JObject obj, bool deptParse = false)
		{
			if (obj == null)
				return null;
			User user = new User();
			user.user_id = obj.GetObject<string>("user_id");
			user.username = obj.GetObject<string>("username");
			user.created_at = obj.GetObject<string>("created_at");
			user.urlname = obj.GetObject<string>("urlname");
			user.avatar = ImageFile.Parse(obj["avatar"] as JObject);
			user.seq = obj.GetObject<int>("seq");
			user.email = obj.GetObject<string>("email");
			user.follower_count = obj.GetObject<int>("follower_count");
			user.following_count = obj.GetObject<int>("following_count");
			user.board_count = obj.GetObject<int>("board_count");
			user.following = obj.GetObject<bool>("following");
			user.like_count = obj.GetObject<int>("like_count");
			user.pin_count = obj.GetObject<int>("pin_count");

			user.boards = Board.ParseList(obj["boards"] as JArray);

			return user;
		}
		public static List<User> ParseList(JArray arry, bool deptParse = false)
		{
			List<User> list = new List<User>();
			if (arry == null)
				return list;
			foreach (JObject obj in arry)
			{
				var item = Parse(obj, deptParse);
				if (item != null)
					list.Add(item);
			}
			return list;
		}
	}
}
