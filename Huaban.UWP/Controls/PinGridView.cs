using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Huaban.UWP.Controls
{
	public class PinGridView : GridView
	{
		public PinGridView()
		{

			this.SizeChanged += PinGridView_SizeChanged;

		}

		private void PinGridView_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			try
			{
				if (e.NewSize.Width <= 500)
					PinWidth = (e.NewSize.Width - 1) / 2 - 6;
				else
					PinWidth = (e.NewSize.Width - 6) / Math.Floor(e.NewSize.Width / 240) - 6;
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

		public static readonly DependencyProperty PinWidthProperty = DependencyProperty.Register("PinWidth", typeof(double), typeof(PinGridView), new PropertyMetadata(235));
	}
}
