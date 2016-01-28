using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huaban.UWP.ViewModels
{
	using Base;
	using Controls;
	using Models;
	using Commands;

	public class ImageViewModel : HBViewModel
	{
		public ImageViewModel(Context context)
			: base(context)
		{

		}
		#region Properties
		private PinListViewModel _PinListViewModel;
		public PinListViewModel PinListViewModel
		{
			get { return _PinListViewModel; }
			set { SetValue(ref _PinListViewModel, value); }
		}
		#endregion

		#region Methods
		public override void OnNavigatedTo(HBNavigationEventArgs e)
		{
			PinListViewModel model = e.Parameter as PinListViewModel;
			if (model != null)
				PinListViewModel = model;
			
		}
		#endregion
	}
}
