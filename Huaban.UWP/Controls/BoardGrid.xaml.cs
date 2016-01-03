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
using System.Threading.Tasks;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Huaban.UWP.Controls
{
	public sealed partial class BoardGrid : UserControl
	{
		public static readonly DependencyProperty CellWidthProperty = DependencyProperty.Register("CellWidth", typeof(double), typeof(BoardGrid), new PropertyMetadata(0.0));
		public double CellWidth { set; get; }
		public string Fuck { set; get; } = "Fuck world!";
		private bool _loading;
		public BoardGrid()
		{
			this.InitializeComponent();
			this.Loaded += BoardGrid_Loaded;
		}

		private async void BoardGrid_Loaded(object sender, RoutedEventArgs e)
		{
			bool isOver = false;
			while (scrollViewer.ComputedVerticalScrollBarVisibility != Visibility.Visible && !isOver)
			{
				isOver = await LoadData();
			}
		}

		private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
		{
			ScrollViewer sv = sender as ScrollViewer;
			if (sv.VerticalOffset > sv.ScrollableHeight - 40 && !_loading)
			{
				await LoadData();
			}
		}
		private async Task<bool> LoadData()
		{
			_loading = true;
			bool isOver = false;
			try
			{
				var incrementalLoading = lwGrid.ItemsSource as ISupportIncrementalLoading;
				if (incrementalLoading != null && incrementalLoading.HasMoreItems)
				{
					await incrementalLoading.LoadMoreItemsAsync((uint)lwGrid.Items.Count);
				}
				else
					isOver = true;
			}
			catch (Exception ex)
			{ }
			finally
			{
				_loading = false;
			}
			return isOver;
		}
	}
}
