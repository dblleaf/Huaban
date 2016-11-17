using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huaban.UWP.Models
{
    internal class Client : IClient
    {

        public string API_ACTIVITIES { get; } = "http://api.huaban.com/message/activities/";
        public string API_ALL { get; } = "http://api.huaban.com/all/";
        public string API_BOARDS { get; } = "http://api.huaban.com/boards/";
        public string API_CATEGORY { get; } = "http://api.huaban.com/categories/";
        public string API_FAVORITE { get; } = "http://api.huaban.com/favorite/";
        public string API_FEEDS { get; } = "http://api.huaban.com/feeds/";
        public string API_Follow { get; } = "http://api.huaban.com/following/";
        public string API_FRIENDS { get; } = "http://api.huaban.com/friends/";
        public string API_HOST { get; } = "https://huaban.com/";
        public string API_MAIN { get; } = "http://api.huaban.com/";
        public string API_ME { get; } = "http://api.huaban.com/users/me";
        public string API_MENTIONS { get; } = "http://api.huaban.com/message/mentions/";
        public string API_MESSAGE { get; } = "http://api.huaban.com/message/";
        public string API_PIN { get; } = "http://api.huaban.com/pins/";
        public string API_POPULAR_PINS { get; } = "http://api.huaban.com/popular/";
        public string API_REPORT { get; } = "http://api.huaban.com/feedback/report/";
        public string API_SEARCH_BOARD { get; } = "http://api.huaban.com/search/boards/";
        public string API_SEARCH_PEOPLE { get; } = "http://api.huaban.com/search/people/";
        public string API_SEARCH_PIN { get; } = "http://api.huaban.com/search/";
        public string API_SHARE { get; } = "http://api.huaban.com/share/";
        public string API_TOKEN { get; } = "https://huaban.com/oauth/access_token/";
        public string API_USER { get; } = "http://api.huaban.com/users/";
        public string API_WEEKLY { get; } = "http://api.huaban.com/weekly/";
        public string Authorization { get; } = "Authorization";
        public string HBIMG { get; set; } = "http://img.hb.aicdn.com/";
        public string LIMIT { get; set; } = "limit";
        public string MAX { get; set; } = "max";
        public string SINCE { get; set; } = "since";
        public int SO_TIMEOUT { get; set; } = 0x4e20;
        public string X_Client_ID { get; set; } = "X-Client-ID";

        public string ClientID { get; set; }
        public string ClientInfo { get { return ClientID + ":" + ClientSecret; } }
        public string ClientSecret { get; set; }
        public string MD { get; set; }
        public string OAuthCallback { get; set; }
    }
}
