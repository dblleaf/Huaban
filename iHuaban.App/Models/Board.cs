using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHuaban.App.Models
{
    public class Board : IModel
    {
        public string board_id { set; get; }
        public string user_id { set; get; }
        public string description { set; get; }
        public string title { set; get; }
        public string category_id { set; get; }
        public string pin_count { set; get; }
        public string follow_count { set; get; }
        public string like_count { set; get; }
        public string created_at { set; get; }
        public string updated_at { set; get; }
        public bool following { set; get; }
        public int seq { set; get; }
        public double Width { set; get; }

        public string KeyId => board_id;
    }
}
