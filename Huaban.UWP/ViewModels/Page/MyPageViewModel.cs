using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huaban.UWP.ViewModels
{
	using Base;
	using Models;
	public class MyPageViewModel : HBViewModel
	{
		public MyPageViewModel(Context context)
			: base(context)
		{
			MyPinListViewModel = new PinListViewModel(context, GetMyPinList);
			LikePinListViewModel = new PinListViewModel(context, GetLikePinList);
			BoardListViewModel = new BoardListViewModel(context, null);

			BoardListViewModel.BoardList = context.BoardList;
		}

		#region Properties

		public PinListViewModel MyPinListViewModel { set; get; }

		public PinListViewModel LikePinListViewModel { set; get; }

		public BoardListViewModel BoardListViewModel { set; get; }

		#endregion

		#region Commands


		#endregion

		#region Methods

		private async Task<IEnumerable<Pin>> GetMyPinList(uint startIndex, int page)
		{
			IsLoading = true;

			List<Pin> list = new List<Pin>();
			try
			{
				list = await Context.API.UserAPI.GetPins(Context.User.user_id, MyPinListViewModel.GetMaxPinID());
				foreach (var item in list)
				{
					item.Width = MyPinListViewModel.ColumnWidth;
					if (item.file != null)
						item.Height = ((MyPinListViewModel.ColumnWidth - 0.8) * item.file.height / item.file.width);
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

		private async Task<IEnumerable<Pin>> GetLikePinList(uint startIndex, int page)
		{
			IsLoading = true;

			List<Pin> list = new List<Pin>();
			try
			{
				list = await Context.API.UserAPI.GetLikePins(Context.User.user_id, LikePinListViewModel.GetMaxSeq());
				foreach (var item in list)
				{
					item.Width = LikePinListViewModel.ColumnWidth;
					if (item.file != null)
						item.Height = ((LikePinListViewModel.ColumnWidth - 0.8) * item.file.height / item.file.width);
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

		public async override void Inited()
		{
			base.Inited();
			await MyPinListViewModel.ClearAndReload();
			await LikePinListViewModel.ClearAndReload();
		}
		#endregion
	}
}
