using System.Collections.Generic;

namespace iHuaban.App.Models
{
    public class User : IModel
    {
        public string user_id { set; get; }
        public string username { set; get; }
        public string urlname { get; set; }
        public string email { get; set; }
        public int pin_count { set; get; }
        public int like_count { set; get; }
        public int board_count { set; get; }
        public int follower_count { set; get; }
        public int muse_board_count { set; get; }
        public int explore_following_count { set; get; }
        public int boards_like_count { set; get; }
        public int following_count { set; get; }
        public string KeyId => user_id;
        public File avatar { set; get; }
        public string typeName => this.GetType().Name;
        public List<Pin> Pins { get; set; }
        public List<Board> Boards { get; set; }
        public List<Pin> Likes { get; set; }
        
        public List<User> Users { get; set; }
    }

    public class PUser : IModel
    {
        public string user_id { get; set; }
        public string category { get; set; }
        public long updated_at { get; set; }
        public string KeyId => updated_at.ToString();
        public string typeName => this.GetType().Name;

        public User user { get; set; }
    }
}
