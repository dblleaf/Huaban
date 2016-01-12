using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.Foundation.Metadata;

namespace Huaban.UWP.ViewModels
{
	using Base;
	using Services;
	using Models;
	using Commands;
	using Views;
	public class ShellPageViewModel : HBViewModel
	{
		public ShellPageViewModel(Context context)
			: base(context)
		{
			NavList.Insert(3, UserItem);
			UserItem.Special = context.IsLogin;
			NavFootList.Insert(0, ThemeModeItem);

			Context.PropertyChanged += Context_PropertyChanged;
			FirstBackVisibility = Visibility.Collapsed;
			Theme = ElementTheme.Dark;
		}

		#region Properties
		private ElementTheme _Theme;
		public ElementTheme Theme {
			get { return _Theme; }
			set { SetValue(ref _Theme, value); }
		}

		private bool _IsPaneOpen;
		public bool IsPaneOpen
		{
			get { return _IsPaneOpen; }
			set { SetValue(ref _IsPaneOpen, value); }
		}

		private NavItemModel UserItem { set; get; }
			= new NavItemModel()
			{
				DestinationPage = "MyPage",
				Label = "我的",
				Symbol = Symbol.People,
				Authorization = true
			};

		public ObservableCollection<NavItemModel> NavList { get; private set; }
			= new ObservableCollection<NavItemModel>(new NavItemModel[] {
				new NavItemModel() { DestinationPage = "HomePage", Label = "发现", Title = "发现", Symbol = Symbol.Find },
				new NavItemModel() { DestinationPage = "FollowingPage", Label = "关注", SymbolChar = '', Authorization = true },
				new NavItemModel() { DestinationPage = "MessagePage", Label = "消息", Title="消息", Symbol = Symbol.Message, Authorization = true }
			});

		private NavItemModel ThemeModeItem { set; get; }
			= new NavItemModel()
			{
				Label = "白天模式",
				SymbolChar = '',//夜间：
				Authorization = true
			};

		public ObservableCollection<NavItemModel> NavFootList { get; private set; }
			= new ObservableCollection<NavItemModel>(new NavItemModel[] {
				new NavItemModel() { DestinationPage = "AboutPage", Label = "关于", Title="关于", Symbol = Symbol.Help },
				new NavItemModel() { DestinationPage = "SettingPage", Label = "设置", Title="设置", Symbol = Symbol.Setting }
			});

		public User User
		{
			get { return Context.User; }
		}

		private Visibility _FirstBackVisibility;
		public Visibility FirstBackVisibility
		{
			get { return _FirstBackVisibility; }
			set
			{
				SetValue(ref _FirstBackVisibility, value);
			}
		}
		private string _Message;
		public string Message
		{
			get { return _Message; }
			set { SetValue(ref _Message, value); }
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
						if (string.IsNullOrEmpty(item.DestinationPage))
						{
							ChangeTheme();
							DisplayAndSaveTheme();
							return;
						}
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
			ReadTheme();
			DisplayAndSaveTheme();
		}
		private async void ReadTheme()
		{
			ElementTheme theme = await StorageHelper.ReadLocal(o =>
			{
				if (string.IsNullOrEmpty(o))
					return ElementTheme.Dark;
				else
				{
					int t = 0;
					int.TryParse(o, out t);
					return (ElementTheme)t;
				}

			});
			Theme = theme;
		}
		private async void Context_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Message")
			{
				try
				{
					await ShellPage.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
					{
						ShowTip(Context.Message);
					});
				}
				catch (Exception ex)
				{

					string aaa = ex.Message;
				}
			}
		}

		DispatcherTimer timer;
		public void ShowTip(string txt)
		{
			FirstBackVisibility = Visibility.Visible;
			Message = txt;
			if (timer == null)
			{
				timer = new DispatcherTimer();
				timer.Interval = TimeSpan.FromSeconds(1.5);
			}
			timer.Tick += Timer_Tick;
			timer.Start();
		}
		private void Timer_Tick(object sender, object e)
		{
			FirstBackVisibility = Visibility.Collapsed;
			timer.Tick -= Timer_Tick;
		}

		private void ChangeTheme()
		{
			if (Theme == ElementTheme.Dark)
				Theme = ElementTheme.Light;
			else
				Theme = ElementTheme.Dark;
		}

		private async void DisplayAndSaveTheme()
		{
			ElementTheme theme = Theme;
			if (theme == ElementTheme.Light)
			{
				ThemeModeItem.Label = "白天模式";
				ThemeModeItem.SymbolChar = '';
			}
			else {
				ThemeModeItem.Label = "夜间模式";
				ThemeModeItem.SymbolChar = '';
			}
			await StorageHelper.SaveLocal(theme);
		}

		#endregion
	}
}
