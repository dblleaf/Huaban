using Dblleaf.UWP.Huaban.Helpers;
using Dblleaf.UWP.Huaban.Models;
using Dblleaf.UWP.Huaban.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Dblleaf.UWP.Huaban.ViewModels
{
    public class Context : ObservableObject
    {
        public NavigationService NavigationService { get; set; }
        private CategoryService CategoryService { set; get; }
        private IClient Client { set; get; }
        private UserService UserService { set; get; }
        public Context(CategoryService categoryService, UserService userService, NavigationService navigationService, IClient client)
        {
            CategoryService = categoryService;
            UserService = userService;
            NavigationService = navigationService;
            Client = client;
        }
        #region Properties

        private User _User;
        public User User
        {
            get { return _User; }
            set { SetValue(ref _User, value); }
        }

        private bool _IsLogin;
        public bool IsLogin
        {
            get { return _IsLogin; }
            set { SetValue(ref _IsLogin, value); }
        }

        public IncrementalLoadingList<Category> CategoryList => new IncrementalLoadingList<Category>(GetCategoryList);

        public ObservableCollection<Category> Categories => new ObservableCollection<Category>();

        private BoardListViewModel _BoardList;
        public BoardListViewModel BoardListVM
        {
            get { return _BoardList; }
            set { SetValue(ref _BoardList, value); }
        }

        private string _Message;
        public string Message
        {
            get { return _Message; }
            private set { SetValue(ref _Message, value); }
        }

        private AppViewBackButtonVisibility _AppViewBackButtonVisibility;
        public AppViewBackButtonVisibility AppViewBackButtonVisibility
        {
            get { return _AppViewBackButtonVisibility; }
            set { SetValue(ref _AppViewBackButtonVisibility, value); }
        }

        #endregion

        #region Commands
        #endregion

        #region Methods
        public async Task SetToken(AuthToken token)
        {
            Client.SetToken(token);

            var user = await UserService.GetSelf();
            BoardListVM = new BoardListViewModel(this, GetBoardList);
            User = user;
            IsLogin = true;

            await StorageHelper.SaveLocal(token);
            await StorageHelper.SaveLocal(user);
        }

        public async Task ClearToken()
        {
            Client.SetToken(null);
            IsLogin = false;
            User = null;
            //ShellViewModel.UserItem.Special = false;
            await StorageHelper.DeleteLocal($"{typeof(AuthToken).Name}.json");
            await StorageHelper.DeleteLocal($"{typeof(User).Name}.json");
        }

        private async Task<IEnumerable<Board>> GetBoardList(uint startIndex, int page)
        {
            BoardListVM.BoardList.NoMore();

            List<Board> list = new List<Board>();
            try
            {
                list = await UserService.GetBoards(User.user_id, BoardListVM.GetMaxSeq());
                if (list.Count == 0)
                    BoardListVM.BoardList.NoMore();
                else
                    BoardListVM.BoardList.HasMore();
                return list;
            }
            catch (Exception ex)
            {
            }

            return list;
        }

        private async Task<IEnumerable<Category>> GetCategoryList(uint startIndex, int page)
        {
            try
            {
                var list = await CategoryService.GetCategoryList();
                foreach (var item in list)
                {
                    Categories.Add(item);
                }
                list.Insert(0, new Category() { name = "最热", nav_link = "/popular/" });
                list.Insert(0, new Category() { name = "最新", nav_link = "/all/" });

                CategoryList.NoMore();
                return list;
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public void ShowTip(string msg)
        {
            Message = msg;
        }

        #endregion
    }
}
