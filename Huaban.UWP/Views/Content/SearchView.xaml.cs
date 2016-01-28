using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Huaban.UWP.Views
{
	using Controls;
	public sealed partial class SearchView : HBControl
	{
		public SearchView()
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
