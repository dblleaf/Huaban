using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.Foundation.Metadata;

namespace Huaban.UWP.ViewModels
{
	using Base;
	using Services;
	using Models;
	using Commands;
	public class ShellPageViewModel : HBViewModel
	{
		public ShellPageViewModel(Context context)
			: base(context)
		{
			NavList.Insert(3, UserItem);
			UserItem.Special = context.IsLogin;
		}

		#region Properties

		private bool _IsPaneOpen;
		public bool IsPaneOpen { get { return _IsPaneOpen; } set { SetValue(ref _IsPaneOpen, value); } }
		private NavItemModel UserItem { set; get; } = new NavItemModel() { DestinationPage = "MyPage", Label = "我的", Symbol = Symbol.People, Authorization = true };
		public ObservableCollection<NavItemModel> NavList { get; private set; } =
			new ObservableCollection<NavItemModel>(new NavItemModel[] {
				new NavItemModel() { DestinationPage = "HomePage", Label = "发现", Title = "发现", Symbol = Symbol.Find },
				new NavItemModel() { DestinationPage = "FollowingPage", Label = "关注", SymbolChar = '', Authorization = true },
				new NavItemModel() { DestinationPage = "MessagePage", Label = "消息", Title="消息", Symbol = Symbol.Message, Authorization = true },
				new NavItemModel() { DestinationPage = "AboutPage", Label = "关于", Title="关于", Symbol = Symbol.Help },
				new NavItemModel() { DestinationPage = "SettingPage", Label = "设置", Title="设置", Symbol = Symbol.Setting }
			});

		public User User
		{
			get { return Context.User; }
		}
		#endregion

		#region Commands

		private DelegateCommand _NavCommand;
		public DelegateCommand NavCommand
		{
			get
			{
				return _NavCommand ?? (_NavCommand = new DelegateCommand(
					async o =>
					{
						IsPaneOpen = false;
						var args = o as ItemClickEventArgs;
						var item = o as NavItemModel;
						if (args == null && item == null)
							return;

						if (args != null)
							item = args.ClickedItem as NavItemModel;
						if (item.Authorization && !Context.IsLogin)
						{
							LoginDialogViewModel login = new LoginDialogViewModel(Context);
							if (await login.Show())
							{
								NotifyPropertyChanged("User");
								UserItem.Special = true;
								Context.NavigationService.MenuNavigateTo(item.DestinationPage);
							}
						}
						else
							Context.NavigationService.MenuNavigateTo(item.DestinationPage);
					},
					o => !IsLoading)
				);
			}
		}
		#endregion

		#region Methods

		public override void Inited()
		{
			base.Inited();

			NavCommand.Execute(NavList[0]);
		}
		#endregion
	}
}
