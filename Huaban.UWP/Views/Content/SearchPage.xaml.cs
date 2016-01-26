using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Huaban.UWP.Views
{
	using Controls;
	public sealed partial class SearchPage : HBControl
	{
		public SearchPage()
		{
			
			this.InitializeComponent();
			this.Loaded += (s, e) => {
				searchBox.Focus(FocusState.Programmatic);
			};
		}

		private void searchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
		{
			
			this.Focus(FocusState.Programmatic);
		}
	}
}
