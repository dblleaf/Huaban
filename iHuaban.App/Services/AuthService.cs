using iHuaban.App.Helpers;
using iHuaban.App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public class AuthService : IAuthService
    {
        private IAuthHttpHelper httpHelper;
        private Context Context;
        private IStorageService storageService;
        public AuthService(IAuthHttpHelper httpHelper, Context context, IStorageService storageService)
        {
            this.httpHelper = httpHelper;
            this.Context = context;
            this.storageService = storageService;
        }

        public async Task<AuthResult> LoginAsync(string userName, string password)
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

                return result;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task LoadMeAsync()
        {
            string cookieJson = storageService.GetSetting("Cookies");
            var cookies = JsonConvert.DeserializeObject<List<Cookie>>(cookieJson);
            this.Context.SetCookie(cookies);
            var user = await httpHelper.GetAsync<User>(Constants.UrlMe);
            if (user != null && !string.IsNullOrWhiteSpace(user.user_id))
            {
                this.Context.User = user;
            }
        }
    }
}
