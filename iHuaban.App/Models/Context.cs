using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace iHuaban.App.Models
{
    public sealed class Context : ObservableObject
    {
        public Action<string> ShowMessageHandler { get; set; }

        private User user;
        public User User
        {
            get { return user; }
            set { SetValue(ref user, value); }
        }

        private Board quickBoard;
        public Board QuickBoard
        {
            get { return quickBoard; }
            set { SetValue(ref quickBoard, value); }
        }

        internal CookieContainer CookieContainer { get; set; } = new CookieContainer();

        public void ShowMessage(string mesage)
        {
            if (ShowMessageHandler != null)
            {
                ShowMessageHandler(mesage);
            }
        }

        public Dictionary<string, string> Cookies { get; set; } = new Dictionary<string, string>();

        public void AddCookies(IEnumerable<KeyValuePair<string, string>> cookies)
        {
            foreach (var cookie in cookies)
            {
                if (this.Cookies.ContainsKey(cookie.Key))
                {
                    this.Cookies[cookie.Key] = cookie.Value;
                }
                else
                {
                    this.Cookies.Add(cookie.Key, cookie.Value);
                }
            }
        }

        public string CookieString
        {
            get
            {
                return string.Join(";", this.Cookies.Select(o => $"{o.Key}={o.Value}"));
            }
        }
    }
}
