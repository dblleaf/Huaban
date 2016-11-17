using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Huaban.UWP.Controls
{
	public sealed partial class FixedHead : UserControl
	{
		public FixedHead()
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
			DependencyProperty.Register("Header", typeof(UIElement), typeof(FixedHead), new PropertyMetadata(null, OnHeaderChanged));

		private static void OnHeaderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var control = (FixedHead)sender;
			control.HeaderContent.Content = e.NewValue;
		}
		#endregion

		#region Content
		public new UIElement Content
		{
			get { return (UIElement)GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}

		public static new readonly DependencyProperty ContentProperty =
			DependencyProperty.Register("Content", typeof(UIElement), typeof(FixedHead), new PropertyMetadata(null, OnContentChanged));

		private static void OnContentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var control = (FixedHead)sender;
			control.contentControl.Content = e.NewValue;
		}
		#endregion
	}
}
