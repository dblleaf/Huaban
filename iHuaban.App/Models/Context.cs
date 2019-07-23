using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
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

        public void AddCookies(string cookies)
        {

        }
    }
}
