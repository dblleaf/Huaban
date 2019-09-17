using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace iHuaban.App.Models
{
    public class AuthToken
    {
        public String access_token;
        public int expires_in;
        public String refresh_token;

        public String token_type;
        public DateTime ExpiresIn { set; get; }

        public static AuthToken ParseByQueryString(string text)
        {
            AuthToken token = null;

            var matchAccessToken = rgxAccessToken.Match(text);
            var matchRefreshToken = rgxRefreshToken.Match(text);
            var matchTokenType = rgxTokenType.Match(text);
            var matchExpiresIn = rgxExpiresIn.Match(text);
            if (matchAccessToken.Success
                && matchRefreshToken.Success)
            {
                token = new AuthToken();
                token.access_token = matchAccessToken.Groups[1].Value;
                token.refresh_token = matchRefreshToken.Groups[1].Value;
                token.token_type = matchTokenType.Groups[1].Value;
                token.expires_in = Convert.ToInt32(matchExpiresIn.Groups[1].Value);
            }

            return token;
        }

        //ms-appx-web:///Assets/Test.html#access_token=c7679035-2e37-4a6c-9bef-3509b3bef1d7&refresh_token=3ebb6512-57a8-4e02-a54a-7721a337822e&token_type=bearer&expires_in=86400
        private static Regex rgxAccessToken = new Regex("access_token=([^&]+)");
        private static Regex rgxRefreshToken = new Regex("refresh_token=([^&]+)");
        private static Regex rgxTokenType = new Regex("token_type=([^&]+)");
        private static Regex rgxExpiresIn = new Regex("expires_in=([0-9]+)");
    }
}
