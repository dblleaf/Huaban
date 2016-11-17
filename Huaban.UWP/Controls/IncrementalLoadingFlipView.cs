using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Huaban.UWP.Controls
{
	public class IncrementalLoadingFlipView : FlipView
	{
		public IncrementalLoadingFlipView()
		{
			this.SelectionChanged += (sender, e) =>
			{
				if (this.SelectedIndex == this.Items.Count - 3)
				{
					ISupportIncrementalLoading list = this.ItemsSource as ISupportIncrementalLoading;
					if (list?.HasMoreItems == true)
						list?.LoadMoreItemsAsync((uint)this.Items.Count);
				}
			};
			
		}

	}
}
