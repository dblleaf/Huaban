using iHuaban.App.Helpers;
using iHuaban.App.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    /// <summary>
    /// Current user's operations.
    /// </summary>
    public class AccountService : IAccountService
    {
        private IAuthHttpHelper httpHelper;
        private Context Context;
        private IStorageService storageService;
        public AccountService(IAuthHttpHelper httpHelper, Context context, IStorageService storageService)
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

        public async Task<User> GetMeAsync()
        {
            return await httpHelper.GetAsync<User>(Constants.UrlMe);
        }

        public async Task<PinResult> PickPinAsync(Pin pin, string boardId, bool check = true)
        {
            string url = "pins/";
            if (check)
            {
                url += "?check=true";
            }
            Dictionary<string, object> body = new Dictionary<string, object>()
            {
                { "board_id", boardId },
                { "text", pin.raw_text },
                { "via", pin.pin_id },
            };

            return await httpHelper.PostAsync<PinResult>(url, null, body);
        }

        public async Task<LikeResult> LikePinAsync(Pin pin)
        {
            return await httpHelper.PostAsync<LikeResult>($"pins/{pin.pin_id}/like/");
        }

        public async Task<FollowBoardResult> FollowBoardAsync(Board board)
        {
            return await httpHelper.PostAsync<FollowBoardResult>($"boards/{board.board_id}/follow/");
        }

        public async Task<string> FollowUserAsync(User user)
        {
            return await httpHelper.PostAsync($"users/{user.user_id}/follow/");
        }

        public async Task<string> UnLikePinAsync(Pin pin)
        {
            return await httpHelper.PostAsync($"pins/{pin.pin_id}/unlike/");
        }

        public async Task<string> UnFollowBoardAsync(Board board)
        {
            return await httpHelper.PostAsync($"boards/{board.board_id}/unfollow/");
        }

        public async Task<string> UnFollowUserAsync(User user)
        {
            string urlName = user.user_id;
            if (!string.IsNullOrWhiteSpace(user.urlname))
            {
                urlName = user.urlname;
            }
            return await httpHelper.PostAsync($"users/{urlName}/unfollow/");
        }

        public async Task<string> CreateBoardAsync(Board board, string category)
        {
            Dictionary<string, object> body = new Dictionary<string, object>()
            {
                { "title", board.title },
                { "category", category },
                { "creation", false },
                { "is_private", false },
            };

            return await httpHelper.PostAsync("boards/", null, body);
        }

        public async Task<BoardCollection> GetLastBoardsAsync()
        {
            return await httpHelper.GetAsync<BoardCollection>("last_boards/");
        }

        public async Task<BoardCollection> GetBoardListAsync()
        {
            var urlName = this.Context.User.user_id;
            if (!string.IsNullOrWhiteSpace(this.Context.User.urlname))
            {

            }
            return await httpHelper.GetAsync<BoardCollection>("last_boards/");
        }
    }
}
