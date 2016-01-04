using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.Foundation;

namespace Huaban.UWP.ViewModels
{
	using Base;
	using Models;
	public class HBViewModel : ViewModelBase
	{
		protected Context Context { get; private set; }
		public HBViewModel(Context context)
		{
			Context = context;
			LeftHeaderVisibility = Visibility.Visible;
		}

		private Visibility _LeftHeaderVisibility;
		public Visibility LeftHeaderVisibility
		{
			get { return _LeftHeaderVisibility; }
			protected set { SetValue(ref _LeftHeaderVisibility, value); }
		}
	
		public override Size ArrangeOverride(Size finalSize)
		{
			if (Window.Current.Bounds.Width >= 720)
				LeftHeaderVisibility = Visibility.Collapsed;
			else
				LeftHeaderVisibility = Visibility.Visible;
			return finalSize;
		}

	}
}
