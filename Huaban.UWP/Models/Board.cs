using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static System.Net.WebUtility;

namespace Huaban.UWP.Models
{
    using Base;
    public class Board : ObservableObject
    {
        public string board_id { set; get; }
        public string user_id { set; get; }
        public string description { set; get; }

        private string _title;
        public string title
        {
            get { return _title; }
            set { SetValue(ref _title, value); }
        }

        public string category_id { set; get; }
        public string pin_count { set; get; }
        public string follow_count { set; get; }
        public string like_count { set; get; }
        public string created_at { set; get; }
        public string updated_at { set; get; }
        public List<Pin> pins { set; get; }
        public User user { set; get; }
        public Pin cover { set; get; }

        private bool _following;
        public bool following
        {
            get { return _following; }
            set { SetValue(ref _following, value); }
        }

        public int seq { set; get; }

        public double Width { set; get; }

        private bool _IsLoaded;
        public bool IsLoaded
        {
            get { return _IsLoaded; }
            set { SetValue(ref _IsLoaded, value); }
        }

        public static Board Parse(JObject obj, bool deptParse = false)
        {
            if (obj == null)
                return null;

            Board board = new Board();
            try
            {
                if (obj.GetObject<int>("is_private") > 0)
                    return null;
                board.board_id = obj.GetObject<string>("board_id");
                board.user_id = obj.GetObject<string>("user_id");
                board.description = obj.GetObject<string>("description");
                board.title = HtmlDecode(obj.GetObject<string>("title"));
                board.category_id = obj.GetObject<string>("category_id");
                board.pin_count = obj.GetObject<string>("pin_count");
                board.follow_count = obj.GetObject<string>("follow_count");
                board.like_count = obj.GetObject<string>("like_count");
                board.created_at = obj.GetObject<string>("created_at");
                board.updated_at = obj.GetObject<string>("updated_at");

                board.following = obj.GetObject<bool>("following");
                board.user = User.Parse(obj["user"] as JObject);
                board.cover = Pin.Parse(obj["cover"] as JObject);
                board.seq = obj.GetObject<int>("seq");

                board.pins = Pin.ParseList(obj["pins"] as JArray);
                if (board.cover == null && board.pins != null && board.pins.Count > 0)
                    board.cover = board.pins[0];

            }
            catch (Exception ex)
            {
                string aaa = ex.Message;
            }
            return board;
        }

        public static List<Board> ParseList(JArray arry, bool deptParse = false)
        {
            List<Board> list = new List<Board>();
            if (arry == null)
                return list;

            foreach (var obj in arry)
            {
                var board = Board.Parse(obj as JObject, deptParse);
                if (board != null)
                    list.Add(board);
            }

            return list;
        }
    }
}
