using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace Huaban.UWP.Base
{
	public class IncrementalLoadingList<T> : ObservableCollection<T>, ISupportIncrementalLoading
	{
		private bool _isBusy = false;

		private Func<uint, int, Task<IEnumerable<T>>> func;

		public bool HasMoreItems
		{
			get; private set;
		}

		public IncrementalLoadingList(Func<uint, int, Task<IEnumerable<T>>> _func)
		{
			Page = 0;
			func = _func;
			this.HasMoreItems = true;
		}

		public async Task ClearAndReload()
		{
			Clear();
			Page = 0;
			await LoadMoreItemsAsync(0);
		}

		public void NoMore()
		{
			this.HasMoreItems = false;
		}

		public void HasMore()
		{
			this.HasMoreItems = true;
		}
		//当前页数
		public int Page { private set; get; }
		public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
		{

			return AsyncInfo.Run(async token =>
			{
				try
				{
					
					if (_isBusy)
					{
						throw new InvalidOperationException("忙着呢，先不搭理你");
					}
					_isBusy = true;

					var _items = await func?.Invoke(count, ++Page);
					foreach (var item in _items)
					{
						this.Add(item);
					}
					_isBusy = false;
				}
				catch { }

				return new LoadMoreItemsResult { Count = (uint)this.Count };
			});
		}
	}
}
