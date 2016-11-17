using System;
using Windows.UI.Xaml.Navigation;

namespace Huaban.UWP.Controls
{
	public class HBNavigatingCancelEventArgs
	{
		private NavigatingCancelEventArgs Args { get; set; }
		private bool _Cancel = true;
		public bool Cancel
		{
			get
			{

				return Args != null ? Args.Cancel : _Cancel;
			}
			set
			{
				if (Args != null)
					Args.Cancel = value;
				else
					_Cancel = value;
			}
		}
		public NavigationMode NavigationMode { get; internal set; }
		public object Parameter { get; internal set; }
		public Type SourcePageType { get; internal set; }
		public static HBNavigatingCancelEventArgs Convert(NavigatingCancelEventArgs e)
		{
			return new HBNavigatingCancelEventArgs()
			{
				Args = e,
				Cancel = e.Cancel,
				NavigationMode = e.NavigationMode,
				Parameter = e.Parameter,
				SourcePageType = e.SourcePageType
			};
		}
	}
}
