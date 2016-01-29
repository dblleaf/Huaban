﻿using System;
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

	public class MyViewModel : HBViewModel
	{
		public MyViewModel(Context context)
			: base(context)
		{

		}
		private static UserViewModel _UserViewModel;
		private DelegateCommand _LoadedCommand;
		public DelegateCommand LoadedCommand
		{
			get
			{
				return _LoadedCommand ?? (_LoadedCommand = new DelegateCommand(
				o =>
				{
					var vm = o as UserViewModel;

					vm?.OnNavigatedTo(new HBNavigationEventArgs()
					{
						NavigationMode = Windows.UI.Xaml.Navigation.NavigationMode.New,
						Parameter = Context.User
					});
					_UserViewModel = vm;
				}, o => true));
			}
		}
	}
}