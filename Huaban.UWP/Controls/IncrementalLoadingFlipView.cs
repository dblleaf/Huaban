using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
