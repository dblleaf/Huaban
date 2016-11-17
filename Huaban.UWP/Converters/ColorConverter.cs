using System;
using Windows.UI.Xaml.Data;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Huaban.UWP.Converters
{
	public class ColorConverter : IValueConverter
	{
		private static int[] RGBArray = new int[3];
		private static Random rdm = new Random();
		private static SolidColorBrush GetBrush()
		{
			int[] _RGBArray = new int[3];
			for (int i = 0; i < 3; i++)
			{
				int r = rdm.Next(125, 255);
				while (Math.Abs(r - RGBArray[i]) < 5 || _RGBArray[i] == 0)
					_RGBArray[i] = (r = rdm.Next(125, 255));
			}
			RGBArray = _RGBArray;
			return new SolidColorBrush(Color.FromArgb(255, (byte)_RGBArray[0], (byte)_RGBArray[1], (byte)_RGBArray[2]));
		}
		/// <summary>
		/// 随机淡雅颜色
		/// </summary>
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return GetBrush();
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
