using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Huaban.UWP.ViewModels
{
	using Base;
	using Commands;
	using Services;
	using Models;
	public class PinListViewModel : HBViewModel
	{
		public PinListViewModel(Context context, Func<uint, int, Task<IEnumerable<Pin>>> _func)
			: base(context)
		{
			PinList = new IncrementalLoadingList<Pin>(_func);
			this.SetWidth(Window.Current.Bounds.Width);
		}

		private double _BindWidth;
		public double BindWidth
		{
			get { return _BindWidth; }
			set { SetValue(ref _BindWidth, value); }
		}
		private double _ColumnWidth;
		public double ColumnWidth
		{
			get { return _ColumnWidth; }
			set { SetValue(ref _ColumnWidth, value); }
		}
		public void SetWidth(double width)
		{
			ColumnWidth = 240 - 6;
			int columncount = Convert.ToInt32(Math.Max(2, Math.Floor(width / 240)));
			double bindWidth = columncount * 240 - 6;
			if (width < 720)
			{
				bindWidth = width - 6;
				ColumnWidth = bindWidth / 2 - 6;
			}
				
			ColumnCount = columncount;
			BindWidth = bindWidth;
		}
		private int _ColumnCount;
		public int ColumnCount
		{
			get { return _ColumnCount; }
			set { SetValue(ref _ColumnCount, value); }
		}

		private IncrementalLoadingList<Pin> _PinList;
		public IncrementalLoadingList<Pin> PinList
		{
			get { return _PinList; }
			set
			{ SetValue(ref _PinList, value); }
		}

		public async Task ClearAndReload()
		{
			PinList.Clear();
			await PinList.LoadMoreItemsAsync(0);
		}

		public int Count
		{
			get { return PinList.Count; }
		}

		public long GetMaxPinID()
		{
			long max = 0;
			if (Count > 0)
				max = Convert.ToInt64(PinList[Count - 1].pin_id);
			return max;
		}
		public long GetMaxSeq()
		{
			long max = 0;
			if (Count > 0)
				max = Convert.ToInt64(PinList[Count - 1].seq);
			return max;
		}
		//ShowPinDetailCommand
		private DelegateCommand _ShowPinDetailCommand;
		public DelegateCommand ShowPinDetailCommand
		{
			get
			{
				return _ShowPinDetailCommand ?? (_ShowPinDetailCommand = new DelegateCommand(
					(Object obj) =>
					{
						ItemClickEventArgs e = obj as ItemClickEventArgs;
						var pin = e.ClickedItem as Pin;
						if (pin != null)
						{
							Context.NavigationService.NavigateTo("PinDetailPage", pin);
							//MainPage.Current.OFrame.Navigate(typeof(Views.PinDetailPage), pin);
						}
					},
					(Object obj) => !IsLoading)
				);
			}
		}
	}
}
