using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Net.WebUtility;

namespace Huaban.UWP.Models
{
    using Base;
    public class Pin : ObservableObject
    {
        public Pin()
        {
            PinLoading = true;
        }
        public string pin_id { set; get; }
        public string user_id { set; get; }
        public string board_id { set; get; }
        public string file_id { set; get; }
        public string seq { set; get; }
        public string media_type { set; get; }
        public string source { set; get; }
        public string link { set; get; }
        public string raw_text { set; get; }
        //public string text_meta { set; get; }
        public string via { set; get; }
        //public int via_user_id { set; get; }
        public string original { set; get; }
        public string created_at { set; get; }
        public string like_count { set; get; }

        public string comment_count { set; get; }
        public string repin_count { set; get; }
        public string orig_source { set; get; }
        public ImageFile file { set; get; }


        public User user { set; get; }
        public Board board { set; get; }
        public List<Pin> repins { set; get; }

        public List<Comment> comments { set; get; }
        public List<User> likes { set; get; }
        //public User via_user { set; get; }
        //public Commodity mCommodity { set; get; }
        //private String CreateDateTime { get { return ""; } }

        public double Width { set; get; }
        public double Height { set; get; }

        private bool _IsLoaded;
        public bool IsLoaded
        {
            get { return _IsLoaded; }
            set
            {
                SetValue(ref _IsLoaded, value);
            }
        }

        private bool _IsLoading;
        public bool PinLoading
        {
            get { return _IsLoading; }
            set { SetValue(ref _IsLoading, value); }
        }

        private bool _liked;
        public bool liked
        {
            get { return _liked; }
            set { SetValue(ref _liked, value); }
        }

        public static List<Pin> ParseList(JArray arr, bool deptParse = false)
        {
            var list = new List<Pin>();
            if (arr == null)
                return list;
            try
            {
                foreach (var obj in arr)
                {
                    var item = Parse(obj as JObject, deptParse);
                    if (item != null)
                        list.Add(item);
                }
            }
            catch (Exception ex)
            {
                string aaa = ex.Message;
            }
            return list;
        }
        public static Pin Parse(JObject obj, bool deptParse = false)
        {
            if (obj == null)
                return null;

            Pin item = new Pin();
            item.pin_id = obj.GetObject<string>("pin_id");
            item.user_id = obj.GetObject<string>("user_id");
            item.board_id = obj.GetObject<string>("board_id");
            item.file_id = obj.GetObject<string>("file_id");
            item.seq = obj.GetObject<string>("seq");
            item.media_type = obj.GetObject<string>("media_type");
            item.source = obj.GetObject<string>("source");
            item.link = obj.GetObject<string>("link");
            item.raw_text = HtmlDecode(obj.GetObject<string>("raw_text"));
            item.via = obj.GetObject<string>("via");
            item.original = obj.GetObject<string>("original");
            item.created_at = obj.GetObject<string>("created_at");
            item.like_count = obj.GetObject<string>("like_count");
            item.comment_count = obj.GetObject<string>("comment_count");
            item.repin_count = obj.GetObject<string>("repin_count");
            item.orig_source = obj.GetObject<string>("orig_source");
            item.file = ImageFile.Parse(obj["file"] as JObject);
            item.liked = obj.GetObject<bool>("liked");
            item.user = User.Parse(obj["user"] as JObject);
            item.board = Board.Parse(obj["board"] as JObject);
            if (deptParse)
            {
                item.repins = Pin.ParseList(obj["repins"] as JArray, false);
                item.comments = Comment.ParseList(obj["repins"] as JArray, false);
                item.likes = User.ParseList(obj["likes"] as JArray, false);
            }
            return item;
        }
    }
    public class PinHelpeClass
    {
        public List<Pin> pins { set; get; }
    }
}
