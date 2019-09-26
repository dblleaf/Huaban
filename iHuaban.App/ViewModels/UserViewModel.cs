using iHuaban.App.Helpers;
using iHuaban.App.Models;
using iHuaban.Core.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace iHuaban.App.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private IApiHttpHelper httpHelper;
        public User User { get; private set; }
        public UserViewModel(User user, IApiHttpHelper httpHelper)
        {
            this.User = user;
            this.httpHelper = httpHelper;
            string urlname = !string.IsNullOrEmpty(this.User.urlname) ? this.User.urlname : this.User.user_id;
            urlname = urlname + "/";
            ListTypes = new List<ViewModelBase>
            {
                new ListViewModel<Pin>
                (
                    dataType: new DataType { Title = "采集", Badge = user.pin_count, BaseUrl = urlname + "pins/" },
                    httpHelper: httpHelper,
                    converter: o => JsonConvert.DeserializeObject<PinCollection>(o).Data,
                    feedKeyfunc: o=> o.pin_id.ToString()
                ),
                new ListViewModel<Board>
                (
                    dataType: new DataType { Title = "画板", Badge = user.board_count, BaseUrl = urlname + "boards/" },
                    httpHelper: httpHelper,
                    converter: o => JsonConvert.DeserializeObject<BoardCollection>(o).Data
                ),
                new ListViewModel<Pin>
                (
                    dataType: new DataType { Title = "喜欢", Badge = user.like_count, BaseUrl = urlname + "likes/" },
                    httpHelper: httpHelper,
                    converter: o => JsonConvert.DeserializeObject<PinCollection>(o).Data,
                    feedKeyfunc: o=> o.seq.ToString()
                ),
                new ListViewModel<User>
                (
                    dataType: new DataType { Title = "粉丝", Badge = user.follower_count, BaseUrl = urlname + "followers/", ScaleSize = "4:5", },
                    httpHelper: httpHelper,
                    converter: o => JsonConvert.DeserializeObject<UserCollection>(o).Data,
                    feedKeyfunc: o=> o.seq.ToString()
                ),
                new ListViewModel<User>
                (
                    dataType: new DataType { Title = "关注用户", Badge = user.following_count, BaseUrl = urlname + "following", ScaleSize = "4:5", },
                    httpHelper: httpHelper,
                    converter: o => JsonConvert.DeserializeObject<UserCollection>(o).Data,
                    feedKeyfunc: o=> o.seq.ToString()
                ),
                new ListViewModel<Board>
                (
                    dataType: new DataType { Title = "关注画板", Badge = user.muse_board_count, BaseUrl = urlname + "following/boards" },
                    httpHelper: httpHelper,
                    converter: o => JsonConvert.DeserializeObject<BoardCollection>(o).Data,
                    type: ListViewModelType.Page
                )
            };
        }

        public List<ViewModelBase> ListTypes { get; set; }

    }
}
