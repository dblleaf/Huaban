using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace iHuaban.App.Models
{
    public sealed class Context : ObservableObject
    {
        public Action<string> ShowMessageHandler { get; set; }
        public Action<Pin> PickPinHandlder { get; set; }
        public Action<Pin> QuickPickPinHandler { get; set; }
        public Action<Pin> LikeHandler { get; set; }
        public Action<Board> FollowBoardHandler { get; set; }
        public Action<User> FollowUserHandler { get; set; }
        public CookieContainer CookieContainer { get; set; } = new CookieContainer();

        private User user;
        public User User
        {
            get { return user; }
            set
            {
                SetValue(ref user, value);
                IsLogin = value != null;
            }
        }

        private Board quickBoard;
        public Board QuickBoard
        {
            get { return quickBoard; }
            set { SetValue(ref quickBoard, value); }
        }

        private bool isLogin;
        public bool IsLogin
        {
            get { return isLogin; }
            set { SetValue(ref isLogin, value); }
        }

        private ElementTheme _PaneRequestTheme;
        public ElementTheme PaneRequestTheme
        {
            get { return _PaneRequestTheme; }
            set { SetValue(ref _PaneRequestTheme, value); }
        }

        private Brush _PaneBrush;
        public Brush PaneBrush
        {
            get { return _PaneBrush; }
            set { SetValue(ref _PaneBrush, value); }
        }

        public void ShowMessage(string mesage)
        {
            if (ShowMessageHandler != null)
            {
                ShowMessageHandler(mesage);
            }
        }

        public void PickPin(Pin pin)
        {
            if (PickPinHandlder != null)
            {
                PickPinHandlder(pin);
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

        public void SetCookie(IEnumerable<Cookie> cookies)
        {
            if (cookies == null)
            {
                return;
            }

            foreach (Cookie cookie in cookies)
            {
                this.CookieContainer.Add(new Uri(Constants.UrlBase), cookie);
            }
        }
    }
}
