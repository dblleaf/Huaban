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
	using ViewModels;
	public sealed partial class ImageGrid : UserControl
	{
		public ImageGrid()
		{
			this.InitializeComponent();
		}

		#region Header
		public UIElement Header
		{
			get { return (UIElement)GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}

		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.Register("Header", typeof(UIElement), typeof(ImageGrid), new PropertyMetadata(null, OnHeaderChanged));

		private static void OnHeaderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var control = (ImageGrid)sender;
			control.HeaderContent.Content = e.NewValue;
		}
		#endregion

		#region ListModel
		public PinListViewModel ListModel
		{
			get { return (PinListViewModel)GetValue(ListModelProperty); }
			set { SetValue(ListModelProperty, value); }
		}

		public static readonly DependencyProperty ListModelProperty =
			DependencyProperty.Register("ListModel", typeof(PinListViewModel), typeof(ImageGrid), new PropertyMetadata(null, OnListModelChanged));

		private static void OnListModelChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var control = (ImageGrid)sender;
			control.lvWf.DataContext = e.NewValue;
		}
		#endregion

		private bool _loading;
		public event RoutedEventHandler RequestData;

		private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
		{
			ScrollViewer sv = sender as ScrollViewer;
			if (sv.VerticalOffset > sv.ScrollableHeight - this.ActualHeight && !_loading)
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
