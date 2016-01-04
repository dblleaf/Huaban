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
	using ViewModels;
	public sealed partial class BoardGrid : UserControl
	{
		private bool _loading;
		public BoardGrid()
		{
			this.InitializeComponent();
			this.Loaded += BoardGrid_Loaded;
		}

		#region Header
		public UIElement Header
		{
			get { return (UIElement)GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}

		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.Register("Header", typeof(UIElement), typeof(BoardGrid), new PropertyMetadata(null, OnHeaderChanged));

		private static void OnHeaderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var control = (BoardGrid)sender;
			control.HeaderContent.Content = e.NewValue;
		}
		#endregion

		#region ListModel
		public BoardListViewModel ListModel
		{
			get { return (BoardListViewModel)GetValue(ListModelProperty); }
			set { SetValue(ListModelProperty, value); }
		}

		public static readonly DependencyProperty ListModelProperty =
			DependencyProperty.Register("ListModel", typeof(BoardListViewModel), typeof(BoardGrid), new PropertyMetadata(null, OnListModelChanged));

		private static void OnListModelChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var control = (BoardGrid)sender;
			control.lwGrid.DataContext = e.NewValue;
		}
		#endregion

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
