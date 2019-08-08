using iHuaban.App.Models;
using iHuaban.Core.Helpers;
using iHuaban.Core.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace iHuaban.App.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private IHttpHelper httpHelper;
        public override string TemplateName => Constants.TemplateCurrentUser;
        public User User { get; private set; }
        public UserViewModel(User user, IHttpHelper httpHelper)
        {
            this.User = user;
            this.httpHelper = httpHelper;
            string urlname = !string.IsNullOrEmpty(this.User.urlname) ? this.User.urlname : this.User.user_id;
            urlname = Constants.UrlBase + urlname + "/";
            new ListViewModel<Pin>
            {
                Title = "采集",
                Badge = user.pin_count,
                BaseUrl = urlname + "pins/",
                TemplateName = Constants.TemplatePinList,
                HttpHelper = httpHelper,
                Converter = o => JObject.Parse(o).GetValue("pins").Values<Pin>()
            };
            new ListViewModel<Board>
            {
                Title = "画板",
                Badge = user.board_count,
                BaseUrl = urlname,
                TemplateName = Constants.TemplateBoardList,
                HttpHelper = httpHelper,
                Converter = o => JObject.Parse(o).GetValue("boards").Values<Board>()
            };
            new ListViewModel<Pin>
            {
                Title = "喜欢",
                Badge = user.like_count,
                BaseUrl = urlname + "likes/",
                TemplateName = Constants.TemplatePinList,
                HttpHelper = httpHelper,
                Converter = o => JObject.Parse(o).GetValue("likes").Values<Pin>()
            };
            new ListViewModel<User>
            {
                Title = "关注用户",
                Badge = user.following_count,
                BaseUrl = urlname + "following",
                TemplateName = Constants.TemplateUserList,
                HttpHelper = httpHelper,
                Converter = o => JObject.Parse(o).GetValue("user").Values<User>()
            };

            new ListViewModel<Board>
            {
                Title = "关注画板",
                Badge = user.muse_board_count,
                BaseUrl = urlname + "following/boards",
                TemplateName = Constants.TemplateBoardList,
                HttpHelper = httpHelper,
                Converter = o => JObject.Parse(o).GetValue("boards").Values<Board>()
            };

            new ListViewModel<User>
            {
                Title = "粉丝",
                Badge = user.follower_count,
                BaseUrl = urlname + "followers/",
                TemplateName = Constants.TemplateUserList,
                HttpHelper = httpHelper,
                Converter = o => JObject.Parse(o).GetValue("users").Values<User>()
            };

            ListTypes = new List<ViewModelBase>
            {
                new ListViewModel<Pin>
                {
                    BaseUrl = urlname+"pins/",
                    TemplateName = Constants.TemplatePinList,
                    HttpHelper = httpHelper,
                    Converter = o => JObject.Parse(o).GetValue("pins").Values<Pin>()
                },
                new ListViewModel<Board>(urlname, Constants.TemplateBoardList, httpHelper, o => JObject.Parse(o).GetValue("boards").Values<Board>()),
                new ListViewModel<Pin>(urlname + "likes/", Constants.TemplatePinList, httpHelper, o => JObject.Parse(o).GetValue("likes").Values<Pin>()),
                new ListViewModel<User>(urlname + "following", Constants.TemplateUserList, httpHelper, o => JObject.Parse(o).GetValue("users").Values<User>()),
                new ListViewModel<Board>(urlname + "following/boards", Constants.TemplateBoardList, httpHelper, o => JObject.Parse(o).GetValue("boards").Values<Board>()),
                new ListViewModel<User>(urlname + "followers/", Constants.TemplateUserList, httpHelper, o => JObject.Parse(o).GetValue("users").Values<User>()),
            };
        }

        public List<ViewModelBase> ListTypes { get; set; }
    }
}
