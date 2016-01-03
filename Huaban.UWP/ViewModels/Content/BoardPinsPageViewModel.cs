using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;

namespace Huaban.UWP.ViewModels
{
	using Models;
	using Controls;
	using Base;

	public class BoardPinsPageViewModel : HBViewModel
	{


		public BoardPinsPageViewModel(Context context)
			: base(context)
		{
			PinListViewModel = new PinListViewModel(context, GetData);
			Title = "画板";
		}

		#region Properties

		public PinListViewModel PinListViewModel { set; get; }

		private Board _CurrentBoard;
		public Board CurrentBoard
		{
			get { return _CurrentBoard; }
			set
			{
				SetValue(ref _CurrentBoard, value);
				Title = _CurrentBoard.title;
			}
		}

		#endregion

		#region Commands


		#endregion

		#region Methods

		private async Task<IEnumerable<Pin>> GetData(uint startIndex, int page)
		{
			if (CurrentBoard == null)
				return new List<Pin>();

			IsLoading = true;
			try
			{
				var list = await Context.API.BoardAPI.GetPins(CurrentBoard.board_id, PinListViewModel.GetMaxPinID());
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
			return null;
		}

		public async override void OnNavigatedTo(HBNavigationEventArgs e)
		{
			var board = e.Parameter as Board;
			if (board != null)
				CurrentBoard = board;
			if (e.NavigationMode == NavigationMode.New)
			{
				await PinListViewModel.ClearAndReload();
			}
		}

		#endregion


	}
}
