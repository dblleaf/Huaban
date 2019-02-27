using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dblleaf.UWP.Huaban.Models
{
    internal class Client : IClient
    {
        public string ClientID { get; set; }
        public string ClientInfo { get { return ClientID + ":" + ClientSecret; } }
        public string ClientSecret { get; set; }
        public string MD { get; set; }
        public string OAuthCallback { get; set; }

        public AuthToken Token { get; set; }
        public void SetToken(AuthToken token)
        {
            Token = token;
        }
    }
}
