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
using Windows.UI.Popups;

namespace Huaban.UWP.ViewModels
{
	using Base;
	using Services;
	using Models;
	using Commands;
	using Views;
	public class ShellViewModel : HBViewModel
	{
		public ShellViewModel(Context context)
			: base(context)
		{
			NavList.Insert(3, UserItem);
			UserItem.Special = context.IsLogin;
			NavFootList.Insert(0, ThemeModeItem);

			Context.PropertyChanged += Context_PropertyChanged;
			FirstBackVisibility = Visibility.Collapsed;
			Setting.Current.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == "DarkMode")
					DisplayTheme();
			};

		}

		#region Properties
		private ElementTheme _Theme;
		public ElementTheme Theme
		{
			get { return _Theme; }
			set { SetValue(ref _Theme, value); }
		}

		private bool _IsPaneOpen;
		public bool IsPaneOpen
		{
			get { return _IsPaneOpen; }
			set
			{
				SetValue(ref _IsPaneOpen, value);
				if (_IsPaneOpen)
					this.Context.NavigationService.BackEvent += NavigationService_BackEvent;
				else
					this.Context.NavigationService.BackEvent -= NavigationService_BackEvent;
			}
		}

		internal static NavItemModel UserItem { set; get; }
			= new NavItemModel()
			{
				DestinationPage = "My",
				Label = "我的",
				SymbolChar = '',
				Authorization = true
			};

		public ObservableCollection<NavItemModel> NavList { get; private set; }
			= new ObservableCollection<NavItemModel>(new NavItemModel[] {
				new NavItemModel() { DestinationPage = "Home", Label = "首页", Title = "首页", Symbol = Symbol.Home },
				new NavItemModel() { DestinationPage = "Following", Label = "关注", SymbolChar = '', Authorization = true },
				new NavItemModel() { DestinationPage = "Message", Label = "消息", Title="消息", Symbol = Symbol.Message, Authorization = true }
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
				new NavItemModel() { DestinationPage = "About", Label = "关于", Title="关于", SymbolChar='' },
				new NavItemModel() { DestinationPage = "Setting", Label = "设置", Title="设置", Symbol = Symbol.Setting }
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
					o =>
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
							return;
						}
						if (item.Authorization && !Context.IsLogin)
						{
							LoginViewModel login = new LoginViewModel(Context, token =>
							{
								NotifyPropertyChanged("User");
								UserItem.Special = true;
								Context.NavigationService.MenuNavigateTo(item.DestinationPage);
							});
							login.Show();
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

		private void NavigationService_BackEvent(object sender, Windows.UI.Core.BackRequestedEventArgs e)
		{
			if (!e.Handled)
			{
				e.Handled = true;
				IsPaneOpen = false;

			}
		}

		public override void Inited()
		{
			base.Inited();

			NavCommand.Execute(NavList[0]);

			Task.Factory.StartNew(async () =>
			{
				await ShellView.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
				{
					try
					{
						DisplayTheme();

						if (string.IsNullOrEmpty(StorageHelper.GetSetting("v1_2_20")))
						{
							string msg = @"1.修复PC端社区账号和花瓣账号不能切换登录的问题

欢迎进QQ群534867173讨论或者提建议，或者发电子邮件到关于的邮箱地址。
";
							var dialog = new MessageDialog(msg, "版本更新 v1.2.20");

							//dialog.Commands.Add(new UICommand("马上下载", async o =>
							//{
							//	await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://pdp/?ProductId=9NBLGGH5PJJT"));
							//}));
							//dialog.Commands.Add(new UICommand("取消"));
							await dialog.ShowAsync();
							StorageHelper.SaveSetting("v1_2_20", "1");
						}

					}
					catch (Exception ex)
					{
						string aaa = ex.Message;
					}

				});

			});


		}


		/// <summary>
		/// 根据Context的Message属性值改变弹出提示框
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void Context_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Message")
			{
				try
				{
					await ShellView.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
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
			Setting.Current.DarkMode = !Setting.Current.DarkMode;

		}

		//根据主题显示不同的菜单
		private void DisplayTheme()
		{
			Theme = Setting.Current.DarkMode ? ElementTheme.Dark : ElementTheme.Light;

			ElementTheme theme = Theme;
			if (theme == ElementTheme.Dark)
			{
				ThemeModeItem.Label = "白天模式";
				ThemeModeItem.SymbolChar = '';
			}
			else {
				ThemeModeItem.Label = "夜间模式";
				ThemeModeItem.SymbolChar = '';
			}

		}

		#endregion
	}
}
