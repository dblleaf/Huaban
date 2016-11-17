using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Huaban.UWP.Controls
{
	using ViewModels;
	public sealed partial class BoardGrid : UserControl
	{
		
		public BoardGrid()
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
			control.gridView.DataContext = e.NewValue;
		}
		#endregion
	}
}
