namespace iHuaban.App.Models
{
    public class User : IModel
    {
        public string user_id { set; get; }
        public string username { set; get; }
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
    }
}
