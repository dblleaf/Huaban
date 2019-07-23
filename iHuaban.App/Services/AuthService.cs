using iHuaban.App.Helpers;
using iHuaban.App.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using iHuaban.Core.Helpers;
using System;

namespace iHuaban.App.Services
{
    public class AuthService : IAuthService
    {
        private IHttpHelper httpHelper;
        public AuthService(IHttpHelper httpHelper)
        {
            this.httpHelper = httpHelper;
        }



        public async Task<bool> LoginAsync(string userName, string password)
        {
            Dictionary<string, string> content = new Dictionary<string, string>
            {
                {"password",password },
                {"email",userName },
                {"_ref","frame" },
            };
            try
            {
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Origin", "https://huaban.com");
                headers.Add("Referer", "https://huaban.com/");
                var result = await httpHelper.PostAsync<AuthResult>(
                    Constants.UrlLogin,
                    headers: headers,
                    content: content);

                if (result.err == "404")
                {
                    return true;
                }

                if (!string.IsNullOrWhiteSpace(result.msg))
                {
                    throw new Exception(result.msg);
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public async Task<User> GetMeAsync()
        {
            return await httpHelper.GetAsync<User>(Constants.UrlMe);
        }

    }
}
