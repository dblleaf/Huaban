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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Huaban.UWP.Controls
{
	public sealed partial class ImageGrid : UserControl
	{
		public ImageGrid()
		{
			this.InitializeComponent();
		}
		private bool _loading;
		public event RoutedEventHandler RequestData;
		
		private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
		{
			ScrollViewer sv = sender as ScrollViewer;
			if (sv.VerticalOffset > sv.ScrollableHeight - 40 && !_loading)
			{
				_loading = true;
				try
				{
					RequestData?.Invoke(this, null);

					var incrementalLoading = lvWf.ItemsSource as ISupportIncrementalLoading;
					if (incrementalLoading != null)
						await incrementalLoading.LoadMoreItemsAsync((uint)lvWf.Items.Count);
				}
				catch (Exception ex)
				{ }
				finally
				{
					_loading = false;
				}
			}
		}

	}
}
