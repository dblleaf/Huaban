using System;
using Windows.UI.Xaml.Controls;

namespace Huaban.UWP.Models
{
	using Base;
	public class NavItemModel : ObservableObject
	{
		public string Label { get; set; }
		public string Title { set; get; }
		public Symbol Symbol
		{
			set
			{
				char c = (char)value;
				SymbolChar = c;
			}
		}
		public char SymbolChar { set; get; }

		public string DestinationPage { get; set; }
		public object Arguments { get; set; }
		public bool Authorization { set; get; }
		private bool _Special;
		public bool Special { set { SetValue(ref _Special, value); } get { return _Special; } }

	}
}
