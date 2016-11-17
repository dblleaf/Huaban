using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Huaban.UWP.ViewModels
{
	using Models;
	using Base;
	using Controls;

	public class PinDetailViewModel : HBViewModel
	{

		public PinDetailViewModel(Context context)
			: base(context)
		{
			BoardListViewModel = new BoardListViewModel(context, GetBoardList);
			UserListViewModel = new UserListViewModel(context, GetLikeList);
			RecommendListViewModel = new PinListViewModel(context, GetRecommendList);
		}
		#region Properties
		public PinListViewModel RecommendListViewModel { set; get; }

		public BoardListViewModel BoardListViewModel { set; get; }

		public UserListViewModel UserListViewModel { set; get; }

		private int _PivotSelectedIndex;
		public int PivotSelectedIndex
		{
			get { return _PivotSelectedIndex; }
			set { SetValue(ref _PivotSelectedIndex, value); }
		}

		private Pin _Pin;
		public Pin Pin
		{
			get { return _Pin; }
			set { SetValue(ref _Pin, value); }
		}

		#endregion

		#region Commands


		#endregion

		#region Methods

		private async Task<IEnumerable<Pin>> GetRecommendList(uint startIndex, int page)
		{
			RecommendListViewModel.PinList.NoMore();

			var list = new List<Pin>();
			if (Pin == null)
				return list;
			IsLoading = true;
			try
			{

				list = await Context.API.PinAPI.GetRecommendList(Pin.pin_id, page);
				foreach (var item in list)
				{
					item.Width = RecommendListViewModel.ColumnWidth;
					if (item.file != null)
						item.Height = ((RecommendListViewModel.ColumnWidth - 0.8) * item.file.height / item.file.width);
				}
				if (list.Count == 0)
					RecommendListViewModel.PinList.NoMore();
				else
					RecommendListViewModel.PinList.HasMore();
				RecommendListViewModel.NotifyPropertyChanged("Count");
				return list;
			}
			catch (Exception ex)
			{
			}
			finally
			{
				IsLoading = false;
			}
			return null;
		}
		private async Task<IEnumerable<Board>> GetBoardList(uint startIndex, int page)
		{
			BoardListViewModel.BoardList.NoMore();
			IsLoading = true;

			List<Board> list = new List<Board>();

			try
			{
				list = await Context.API.PinAPI.GetRelatedBoards(Pin?.pin_id, BoardListViewModel.GetMaxSeq());

				if (list.Count == 0)
					BoardListViewModel.BoardList.NoMore();
				else
					BoardListViewModel.BoardList.HasMore();
				BoardListViewModel.NotifyPropertyChanged("Count");
			}
			catch (Exception ex)
			{
				string a = ex.Message;
			}
			finally
			{
				IsLoading = false;
			}
			return list;
		}

		private async Task<IEnumerable<User>> GetLikeList(uint startIndex, int page)
		{

			UserListViewModel.UserList.NoMore();
			IsLoading = true;

			List<User> list = new List<User>();

			try
			{
				list = await Context.API.PinAPI.GetLikeList(Pin.pin_id, UserListViewModel.GetMaxID());

				if (list.Count == 0)
					UserListViewModel.UserList.NoMore();
				else
					UserListViewModel.UserList.HasMore();

				UserListViewModel.NotifyPropertyChanged("Count");
			}
			catch (Exception ex)
			{
				string a = ex.Message;
			}
			finally
			{
				IsLoading = false;
			}
			return list;
		}

		public async override void OnNavigatedTo(HBNavigationEventArgs e)
		{
			var pin = e.Parameter as Pin;

			if (e.NavigationMode == NavigationMode.New)
			{

				if (pin?.pin_id != Pin?.pin_id)
				{
					Pin = await Context.API.PinAPI.GetPin(pin.pin_id);
					await RecommendListViewModel.PinList.LoadMoreItemsAsync(0);
					await Task.Delay(500);
					await RecommendListViewModel.PinList.LoadMoreItemsAsync(0);
				}

				PivotSelectedIndex = 0;
			}
		}

		
		#endregion
	}
}
