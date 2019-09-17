using iHuaban.Core.Models;

namespace iHuaban.App.Models
{
    public class Pin : ObservableObject, IModel
    {
        public long pin_id { set; get; }
        public long user_id { set; get; }
        public long board_id { set; get; }
        public long file_id { set; get; }
        public int media_type { set; get; }
        public string source { set; get; }
        public string link { set; get; }
        public string raw_text { set; get; }
        public long via { set; get; }
        public long via_user_id { set; get; }
        public long? original { set; get; }
        public long created_at { set; get; }
        public int like_count { set; get; }
        public int comment_count { set; get; }
        public int repin_count { set; get; }
        public long PinId => pin_id;
        public File file { set; get; }
        public string KeyId => pin_id.ToString();

        public string typeName => this.GetType().Name;

        private bool _like;
        public bool like
        {
            get { return _like; }
            set
            {
                SetValue(ref _like, value);
                NotifyPropertyChanged(nameof(dislike));
            }
        }

        public bool dislike
        {
            get { return !_like; }
        }
    }
}
