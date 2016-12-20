using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using Windows.UI;

namespace Huaban.UWP.Views
{
	using Controls;
	using ViewModels;
	using Models;
	public sealed partial class ImageView : HBControl
	{
		public ImageView()
		{
			this.InitializeComponent();
			
		}
		public HBViewModel HBVM
		{
			get { return this.ViewModel as HBViewModel; }
		}

		private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
		{
			if (bottomBar.Visibility == Visibility.Visible)
				bottomBar.Visibility = Visibility.Collapsed;
			else
				bottomBar.Visibility = Visibility.Visible;
		}

		private void IncrementalLoadingFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				var fv = sender as FlipView;
				var fvi = fv.ContainerFromItem(fv.SelectedItem) as FlipViewItem;
				if (fvi == null)
					return;

				var a = this;
				var scrollView = fvi.GetChild<ScrollViewer>("scrollViewer");
				var imgView = fvi.GetChild<Image>("imgView");
				if (scrollView == null || imgView == null)
					return;
				Pin pin = scrollView.DataContext as Pin;
				if (pin == null)
					return;
				imgView.MaxHeight = scrollView.ActualHeight;
				imgView.MaxWidth = Math.Min(pin.file.width, scrollView.ActualWidth);
				scrollView.ChangeView(null, null, 1);
			}
			catch (Exception ex)
			{ }
		}

		private void scrollView_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			try
			{
				var scrollView = sender as ScrollViewer;
				Pin pin = scrollView.DataContext as Pin;
				var imgView = scrollView.GetChild<Image>("imgView");
				if (scrollView == null)
					return;
				imgView.MaxHeight = scrollView.ActualHeight;
				imgView.MaxWidth = Math.Min(pin.file.width, scrollView.ActualWidth);
			}
			catch (Exception ex)
			{

			}
		}
	}
}
