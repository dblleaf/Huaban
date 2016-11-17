using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Huaban.UWP.ViewModels
{
	using Base;
	using Models;

	public class FollowingViewModel : HBViewModel
	{
		public FollowingViewModel(Context context)
			: base(context)
		{
			Title = "";
			PinListViewModel = new PinListViewModel(context, GetPinList);
			BoardListViewModel = new BoardListViewModel(context, GetBoardList);
		}

		#region Properties

		public BoardListViewModel BoardListViewModel { set; get; }

		public PinListViewModel PinListViewModel { set; get; }

		#endregion

		#region Commands


		#endregion

		#region Methods

		private async Task<IEnumerable<Pin>> GetPinList(uint startIndex, int page)
		{
			IsLoading = true;

			List<Pin> list = new List<Pin>();
			try
			{
				list = await Context.API.UserAPI.GetFollowing(PinListViewModel.GetMaxPinID());
				foreach (var item in list)
				{
					item.Width = PinListViewModel.ColumnWidth;
					if (item.file != null)
						item.Height = ((PinListViewModel.ColumnWidth - 0.8) * item.file.height / item.file.width);
				}

				if (list.Count == 0)
					PinListViewModel.PinList.NoMore();
				else
					PinListViewModel.PinList.HasMore();

				return list;
			}
			catch (Exception ex)
			{
			}
			finally
			{
				IsLoading = false;
			}
			return list;
		}

		private async Task<IEnumerable<Board>> GetBoardList(uint startIndex, int page)
		{
			IsLoading = true;
			BoardListViewModel.BoardList.NoMore();

			List<Board> list = new List<Board>();
			try
			{
				list = await Context.API.UserAPI.GetFollowingBoardList(Context.User.urlname, page);
				if (list.Count == 0)
					BoardListViewModel.BoardList.NoMore();
				else
					BoardListViewModel.BoardList.HasMore();
				return list;
			}
			catch (Exception ex)
			{
			}
			finally
			{
				IsLoading = false;
			}
			return list;
		}

		public async override void Inited()
		{
			base.Inited();
			await PinListViewModel.ClearAndReload();
		}
		#endregion
	}
}
