using iHuaban.App.Models;
using iHuaban.App.TemplateSelectors;
using iHuaban.Core.Helpers;
using iHuaban.Core.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

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
            ListTypes = new List<ViewModelBase>
            {
                new ListViewModel<Pin>
                (
                    title: "采集",
                    badge: user.pin_count,
                    baseUrl: urlname + "pins/",
                    templateName: Constants.TemplatePinList,
                    httpHelper: httpHelper,
                    converter: o =>
                    {
                        var obj = JObject.Parse(o).GetValue("user");
                        var pins = obj.Values<Pin>("pins").ToList();
                        return pins;
                    }
                ),
                new ListViewModel<Board>
                (
                    title: "画板",
                    badge: user.board_count,
                    baseUrl: urlname,
                    templateName: Constants.TemplateBoardList,
                    httpHelper: httpHelper,
                    converter: o => JObject.Parse(o).GetValue("boards").Values<Board>()
                ),
                new ListViewModel<Pin>
                (
                    title: "喜欢",
                    badge: user.like_count,
                    baseUrl: urlname + "likes/",
                    templateName: Constants.TemplatePinList,
                    httpHelper: httpHelper,
                    converter: o => JObject.Parse(o).GetValue("likes").Values<Pin>()
                ),
                new ListViewModel<User>
                (
                    title: "关注用户",
                    badge: user.following_count,
                    baseUrl: urlname + "following",
                    templateName: Constants.TemplateUserList,
                    httpHelper: httpHelper,
                    converter: o => JObject.Parse(o).GetValue("user").Values<User>()
                ),
                new ListViewModel<Board>
                (
                    title: "关注画板",
                    badge: user.muse_board_count,
                    baseUrl: urlname + "following/boards",
                    templateName: Constants.TemplateBoardList,
                    httpHelper: httpHelper,
                    converter: o => JObject.Parse(o).GetValue("boards").Values<Board>()
                ),
                new ListViewModel<User>
                (
                    title: "粉丝",
                    badge: user.follower_count,
                    baseUrl: urlname + "followers/",
                    templateName: Constants.TemplateUserList,
                    httpHelper: httpHelper,
                    converter: o => JObject.Parse(o).GetValue("users").Values<User>()
                )
            };
            //ListTypes = new List<ViewModelBase>
            //{
            //    new ListViewModel<Pin>
            //    {
            //        baseUrl: urlname+"pins/",
            //        TemplateName = Constants.TemplatePinList,
            //        HttpHelper = httpHelper,
            //        Converter = o => JObject.Parse(o).GetValue("pins").Values<Pin>()
            //    },
            //    new ListViewModel<Board>(urlname, Constants.TemplateBoardList, httpHelper, o => JObject.Parse(o).GetValue("boards").Values<Board>()),
            //    new ListViewModel<Pin>(urlname + "likes/", Constants.TemplatePinList, httpHelper, o => JObject.Parse(o).GetValue("likes").Values<Pin>()),
            //    new ListViewModel<User>(urlname + "following", Constants.TemplateUserList, httpHelper, o => JObject.Parse(o).GetValue("users").Values<User>()),
            //    new ListViewModel<Board>(urlname + "following/boards", Constants.TemplateBoardList, httpHelper, o => JObject.Parse(o).GetValue("boards").Values<Board>()),
            //    new ListViewModel<User>(urlname + "followers/", Constants.TemplateUserList, httpHelper, o => JObject.Parse(o).GetValue("users").Values<User>()),
            //};
        }

        public List<ViewModelBase> ListTypes { get; set; }

        public DataTemplateSelector DataTemplateSelector => new SupperDataTemplateSelector();
    }
}
