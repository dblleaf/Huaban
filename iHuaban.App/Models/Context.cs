using iHuaban.Core.Models;

namespace iHuaban.App.Models
{
    public sealed class Context : ObservableObject
    {
        public string Sid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Cookie))
                {
                    return string.Empty;
                }
                var arr = Cookie.Split(';');
                if (arr?.Length > 0)
                {
                    return arr[0];
                }
                return string.Empty;
            }
        }
        public string Cookie { get; set; }
        private User user;
        public User User
        {
            get { return user; }
            set { SetValue(ref user, value); }
        }

        private AuthToken authToken;
        public AuthToken AuthToken
        {
            get { return authToken; }
            set { SetValue(ref authToken, value); }
        }

        private Board quickBoard;
        public Board QuickBoard
        {
            get { return quickBoard; }
            set { SetValue(ref quickBoard, value); }
        }
    }
}
