using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Huaban.UWP.Models
{
    public class Comment
    {
        public string comment_id { set; get; }
        public string pin_id { set; get; }
        public string user_id { set; get; }
        public string raw_text { set; get; }
        public int created_at { set; get; }
        public User user { set; get; }

        public static List<Comment> ParseList(JArray arry, bool deptParse)
        {
            return new List<Comment>();
        }
    }
}
