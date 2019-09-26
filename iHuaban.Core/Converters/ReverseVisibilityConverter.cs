using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace iHuaban.Core.Converters
{
    public class ReverseVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Visibility visibility = Visibility.Visible;
            if (value is bool)
            {
                if ((bool)value)
                    visibility = Visibility.Visible;
                else
                    visibility = Visibility.Collapsed;
            }
            else if (value is string)
            {
                if (string.IsNullOrWhiteSpace((string)value) || (string)value == "0")
                    visibility = Visibility.Collapsed;
                else
                    visibility = Visibility.Visible;
            }
            else if (value is int)
            {
                if ((int)value > 0)
                    visibility = Visibility.Visible;
                else
                    visibility = Visibility.Collapsed;
            }
            else if (value is Visibility)
            {
                if ((Visibility)value == Visibility.Collapsed)
                    visibility = Visibility.Visible;
                else
                    visibility = Visibility.Collapsed;
            }
            else
            {
                visibility = Visibility.Visible;
            }
            if (visibility == Visibility.Visible)
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
