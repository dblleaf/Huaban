using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Huaban.UWP.ViewModels
{
	using Base;
	using Models;
	using Commands;
	public class HBViewModel : ViewModelBase
	{
		protected Context Context { get; private set; }
		public HBViewModel(Context context)
		{
			Context = context;
			LeftHeaderVisibility = Visibility.Visible;
		}

		public bool IsLogin
		{
			get { return Context.IsLogin; }
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
		public string TargetName { set; get; }
		#region Commands

		private DelegateCommand _ToBoardPinsCommand;
		public DelegateCommand ToBoardPinsCommand
		{
			get
			{
				return _ToBoardPinsCommand ?? (_ToBoardPinsCommand = new DelegateCommand(
					(Object obj) =>
					{
						var args = obj as ItemClickEventArgs;
						var item = obj as Board;
						if (args == null && item == null)
							return;

						if (args != null)
							item = args.ClickedItem as Board;
						if (item != null)
						{
							Context.NavigationService.NavigateTo("BoardPinsPage", item);
						}
					},
					(Object obj) => !IsLoading)
				);
			}
		}

		//ToPinDetailCommand
		private DelegateCommand _ToPinDetailCommand;
		public DelegateCommand ToPinDetailCommand
		{
			get
			{
				return _ToPinDetailCommand ?? (_ToPinDetailCommand = new DelegateCommand(
					(Object obj) =>
					{
						var args = obj as ItemClickEventArgs;
						var item = obj as Pin;
						if (args == null && item == null)
							return;

						if (args != null)
							item = args.ClickedItem as Pin;

						if (item != null)
						{
							Context.NavigationService.NavigateTo("PinDetailPage", item, "PinDetail");
						}
					},
					(Object obj) => !IsLoading)
				);
			}
		}


		//ToPinDetailCommand
		private DelegateCommand _ToUserPageCommand;
		public DelegateCommand ToUserPageCommand
		{
			get
			{
				return _ToUserPageCommand ?? (_ToUserPageCommand = new DelegateCommand(
					(Object obj) =>
					{
						var args = obj as ItemClickEventArgs;
						var item = obj as User;
						if (args == null && item == null)
							return;

						if (args != null)
							item = args.ClickedItem as User;

						if (item != null)
						{
							Context.NavigationService.NavigateTo("UserPage", item);
						}
					},
					(Object obj) => !IsLoading)
				);
			}
		}
		#endregion
	}
}
