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

	public class MyPageViewModel : HBViewModel
	{
		public MyPageViewModel(Context context)
			: base(context)
		{

		}
		private static UserPageViewModel _UserPageViewModel;
		private DelegateCommand _LoadedCommand;
		public DelegateCommand LoadedCommand
		{
			get
			{
				return _LoadedCommand ?? (_LoadedCommand = new DelegateCommand(
				o =>
				{
					var vm = o as UserPageViewModel;

					vm?.OnNavigatedTo(new HBNavigationEventArgs()
					{
						NavigationMode = Windows.UI.Xaml.Navigation.NavigationMode.New,
						Parameter = Context.User
					});
					_UserPageViewModel = vm;
				}, o => true));
			}
		}
	}
}
