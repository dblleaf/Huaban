using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huaban.UWP.Models
{
	public interface IClient
	{
		string API_ACTIVITIES { get; }
		string API_ALL { get; }
		string API_BOARDS { get; }
		string API_CATEGORY { get; }
		string API_FAVORITE { get; }
		string API_FEEDS { get; }
		string API_FRIENDS { get; }
		string API_Follow { get; }
		string API_HOST { get; }
		string API_MAIN { get; }
		string API_ME { get; }
		string API_MENTIONS { get; }
		string API_MESSAGE { get; }
		string API_PIN { get; }
		string API_POPULAR_PINS { get; }
		string API_REPORT { get; }
		string API_SEARCH_BOARD { get; }
		string API_SEARCH_PEOPLE { get; }
		string API_SEARCH_PIN { get; }
		string API_SHARE { get; }
		string API_TOKEN { get; }
		string API_USER { get; }
		string API_WEEKLY { get; }
		string Authorization { get; }
		string HBIMG { get; }
		string LIMIT { get; }
		string MAX { get; }
		string SINCE { get; }
		int SO_TIMEOUT { get; }
		string X_Client_ID { get; }
		string ClientID { get; set; }
		string ClientSecret { get; set; }
		string ClientInfo { get; }
		string OAuthCallback { get; set; }
		string MD { get; set; }

		AuthToken Token { get; set; }
		void SetToken(AuthToken token);
	}
}
