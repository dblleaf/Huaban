using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Huaban.UWP.Converters
{
	using Services;
	using Base;
	public class LoginVisiblityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return ServiceLocator.Resolve<Context>().IsLogin ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
