using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Huaban.UWP.Converters
{
	public class ReverseVisbilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value == null)
				return Visibility.Visible;

			if (value is bool)
			{
				if ((bool)value)
					return Visibility.Collapsed;
				else
					return Visibility.Visible;
			}
			else if (value is string)
			{
				if (string.IsNullOrWhiteSpace((string)value) || (string)value == "0")
					return Visibility.Visible;
				else
					return Visibility.Collapsed;
			}
			else if (value is int)
			{
				if ((int)value > 0)
					return Visibility.Collapsed;
				else
					return Visibility.Visible;
			}
			else if (value is Visibility)
			{
				if ((Visibility)value == Visibility.Collapsed)
					return Visibility.Visible;
				else
					return Visibility.Collapsed;
			}
			else {
				return Visibility.Visible;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
