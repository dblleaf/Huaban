using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Huaban.UWP.Controls
{
	using ViewModels;
	public sealed partial class UserGrid : UserControl
	{
		private bool _loading;
		public UserGrid()
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
			DependencyProperty.Register("Header", typeof(UIElement), typeof(UserGrid), new PropertyMetadata(null, OnHeaderChanged));

		private static void OnHeaderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var control = (UserGrid)sender;
			control.HeaderContent.Content = e.NewValue;
		}
		#endregion

		#region ListModel
		public UserListViewModel ListModel
		{
			get { return (UserListViewModel)GetValue(ListModelProperty); }
			set { SetValue(ListModelProperty, value); }
		}

		public static readonly DependencyProperty ListModelProperty =
			DependencyProperty.Register("ListModel", typeof(UserListViewModel), typeof(UserGrid), new PropertyMetadata(null, OnListModelChanged));

		private static void OnListModelChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var control = (UserGrid)sender;
			control.gridView.DataContext = e.NewValue;
		}

		#endregion

	}
}
