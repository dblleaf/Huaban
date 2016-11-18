using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Huaban.UWP.Converters
{
    using Services;
    using Base;
	public class OwnerVisiblityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			string userID = (string)value;
			Visibility visibility = Visibility.Collapsed;
			if (!string.IsNullOrEmpty(userID))
			{
				if (userID == ServiceLocator.Resolve<Context>()?.User?.user_id)
				{
					visibility = Visibility.Visible;
				}
			}
			return visibility;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
