using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.UI.Core;

namespace Huaban.UWP.Base
{
	using Models;
	using Services;
	using ViewModels;
	public class Context : ObservableObject
	{
		public Context()
		{
			NavigationService = new NavigationService(this);
			CategoryList = new IncrementalLoadingList<Category>(GetCategoryList);
			Categories = new ObservableCollection<Category>();
		}
		#region Properties
		public NavigationService NavigationService { get; private set; }

		public API API { private set; get; } = API.Current();

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

		public IncrementalLoadingList<Category> CategoryList { get; private set; }

		private ObservableCollection<Category> _Categories;
		public ObservableCollection<Category> Categories
		{
			get { return _Categories; }
			set { SetValue(ref _Categories, value); }
		}

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
			BoardListVM = new BoardListViewModel(this, GetBoardList);
			API.SetToken(token);
			var user = await API.UserAPI.GetSelf();

			User = user;
			IsLogin = true;

			await StorageHelper.SaveLocal(token);
			await StorageHelper.SaveLocal(user);
		}

		public async Task ClearToken()
		{
			API.SetToken(null);
			IsLogin = false;
			User = null;
			ShellViewModel.UserItem.Special = false;
			await StorageHelper.DeleteLocal($"{typeof(AuthToken).Name}.json");
			await StorageHelper.DeleteLocal($"{typeof(User).Name}.json");
		}
		private async Task<IEnumerable<Board>> GetBoardList(uint startIndex, int page)
		{
			BoardListVM.BoardList.NoMore();

			List<Board> list = new List<Board>();
			try
			{
				list = await API.UserAPI.GetBoards(User.user_id, BoardListVM.GetMaxSeq());
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
				var list = await API.CategoryAPI.GetCategoryList();
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
