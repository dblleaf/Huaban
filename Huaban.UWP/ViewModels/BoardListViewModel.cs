using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Huaban.UWP.ViewModels
{
	using Base;
	using Models;
	using Commands;

	public class BoardListViewModel : HBViewModel
	{
		public BoardListViewModel(Context context, Func<uint, int, Task<IEnumerable<Board>>> _func)
			: base(context)
		{
			BoardList = new IncrementalLoadingList<Board>(_func);
		}

		#region Properties

		private IncrementalLoadingList<Board> _BoardList;
		public IncrementalLoadingList<Board> BoardList
		{
			get { return _BoardList; }
			set
			{ SetValue(ref _BoardList, value); }
		}

		public int Count
		{
			get { return BoardList.Count; }
		}

		#endregion

		#region Commands

		private DelegateCommand _ToBoardPinsCommand;
		public DelegateCommand ToBoardPinsCommand
		{
			get
			{
				return _ToBoardPinsCommand ?? (_ToBoardPinsCommand = new DelegateCommand(
					(Object obj) =>
					{
						ItemClickEventArgs e = obj as ItemClickEventArgs;
						var board = e.ClickedItem as Board;
						if (board != null)
						{
							Context.NavigationService.NavigateTo("BoardPinsPage", board);
						}
					},
					(Object obj) => !IsLoading)
				);
			}
		}

		#endregion

		#region Methods

		public async Task ClearAndReload()
		{
			BoardList.Clear();
			await BoardList.LoadMoreItemsAsync(0);
		}

		public long GetMaxSeq()
		{
			long max = 0;
			if (Count > 0)
				max = Convert.ToInt64(BoardList[Count - 1].seq);
			return max;
		}

		public long GetMaxID()
		{
			long max = 0;
			if (Count > 0)
				max = Convert.ToInt64(BoardList[Count - 1].board_id);
			return max;
		}

		#endregion
	}
}
