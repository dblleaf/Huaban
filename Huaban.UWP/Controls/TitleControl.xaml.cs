using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Huaban.UWP.Controls
{
	public sealed partial class TitleControl : UserControl
	{
		public TitleControl()
		{

			this.InitializeComponent();
		}
		public new UIElement Content
		{
			get { return (UIElement)GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}

		public static new readonly DependencyProperty ContentProperty =
			DependencyProperty.Register("Content", typeof(UIElement), typeof(TitleControl), new PropertyMetadata(null, OnContentChanged));

		private static void OnContentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var control = (TitleControl)sender;
			control.contentControl.Content = e.NewValue;
		}
	}
}
