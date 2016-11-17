using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.Foundation.Metadata;

namespace Huaban.UWP.Controls
{
	public class ExtendGridView : GridView
	{
		public ExtendGridView()
		{

			this.SizeChanged += PinGridView_SizeChanged;

		}

		private void PinGridView_SizeChanged(object sender, SizeChangedEventArgs e)
		{

			try
			{
				double width = Math.Floor(e.NewSize.Width);

				double col = Math.Floor(width / 240);
				if (col <= 1)
					col = 2;

				double w = 13;
				if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
					w = 6.1;

				PinWidth = Math.Floor((width - w) * 2 / col) / 2 - 6;
			}
			catch (Exception ex)
			{
				string a = ex.Message;
			}

		}

		public double PinWidth
		{
			get
			{
				return (int)this.GetValue(PinWidthProperty);
			}
			set
			{
				this.SetValue(PinWidthProperty, value);
			}
		}

		public static readonly DependencyProperty PinWidthProperty = DependencyProperty.Register("PinWidth", typeof(double), typeof(ExtendGridView), new PropertyMetadata(235));
	}
}
