using System;
using Windows.UI.Xaml.Navigation;

namespace Huaban.UWP.Controls
{
	public class HBNavigationEventArgs
	{
		public object Content { get; set; }
		public NavigationMode NavigationMode { get; set; }
		public object Parameter { get; set; }
		public Type SourcePageType { get; internal set; }

		public static HBNavigationEventArgs Convert(NavigationEventArgs e)
		{
			return new HBNavigationEventArgs()
			{
				Content = e.Content,
				NavigationMode = e.NavigationMode,
				Parameter = e.Parameter,
				SourcePageType = e.SourcePageType
			};
		}
	}
}
