using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dblleaf.UWP.Huaban.Models
{
    public interface IClient
    {
        string ClientID { get; set; }
        string ClientSecret { get; set; }
        string ClientInfo { get; }
        string OAuthCallback { get; set; }
        string MD { get; set; }

        AuthToken Token { get; set; }
        void SetToken(AuthToken token);
    }
}
