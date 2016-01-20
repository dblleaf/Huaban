using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huaban.UWP.ViewModels
{
	using Base;
	using Models;
	using Windows.Foundation;

	public class FollowingPageViewModel : HBViewModel
	{
		public FollowingPageViewModel(Context context)
			: base(context)
		{
			Title = "";
			PinListViewModel = new PinListViewModel(context, GetPinList);
			BoardListViewModel = new BoardListViewModel(context, GetBoardList);
		}

		#region Properties

		public BoardListViewModel BoardListViewModel { set; get; }

		public PinListViewModel PinListViewModel { set; get; }

		private double _BindWidth;
		public double BindWidth
		{
			get { return _BindWidth; }
			set
			{ SetValue(ref _BindWidth, value); }
		}


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

		public override Size ArrangeOverride(Size finalSize)
		{
			PinListViewModel.SetWidth(finalSize.Width);
			return base.ArrangeOverride(finalSize);
		}

		public async override void Inited()
		{
			base.Inited();
			await PinListViewModel.ClearAndReload();
		}
		#endregion
	}
}
