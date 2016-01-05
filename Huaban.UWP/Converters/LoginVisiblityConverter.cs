using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Huaban.UWP.Converters
{
	public class LoginVisiblityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return App.AppContext.IsLogin? Visibility.Visible: Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
