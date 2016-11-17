using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Huaban.UWP.ViewModels
{
	using Base;
	using Models;

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

		#endregion

		#region Methods

		public async Task ClearAndReload()
		{
			await BoardList.ClearAndReload();
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

		public override void Dispose()
		{
			BoardList.Clear();
			base.Dispose();
		}
		#endregion
	}
}
