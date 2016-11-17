using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Huaban.UWP.ViewModels
{
	using Commands;
	using Views;
	using Base;
	using Models;

	public class LoginViewModel : HBViewModel
	{

		public LoginViewModel(Context context, Action<AuthToken> successAction)
			: base(context)
		{
			Title = "登录";
			LeftHeaderVisibility = Windows.UI.Xaml.Visibility.Collapsed;
			SuccessAction = successAction;

			UserName = StorageHelper.GetSetting("username");
			Password = StorageHelper.GetSetting("password");
			WebViewVisibility = Visibility.Collapsed;
			Popup = new Popup();
			Popup.Child = new LoginView();
			Popup.DataContext = this;

			Loading += () =>
			{
				_LoginCommand.RaiseCanExecuteChanged();
			};
		}
		#region Properties
		protected Popup Popup { set; get; }
		private Action<AuthToken> SuccessAction;
		public List<SNSType> SnsTypes { private set; get; } = new List<SNSType>(new SNSType[] {
			new SNSType { strName = "微博", strType = "weibo", Icon = "ms-appx:///Assets/sina_weibo_50px.png", Url = "http://huaban.com/oauth/weibo/?auth_callback=ms-appx-web:///Assets/Test.html&client_id=1d912cae47144fa09d88&md=com.huaban.android" },
			new SNSType { strName = "QQ", strType = "qzone", Icon = "ms-appx:///Assets/qq_50px.png", Url = "http://huaban.com/oauth/qzone/?auth_callback=ms-appx-web:///Assets/Test.html&client_id=1d912cae47144fa09d88&md=com.huaban.android" },
			new SNSType { strName = "豆瓣", strType = "douban", Icon = "ms-appx:///Assets/douban_50px.png", Url = "https://huaban.com/oauth/douban/?auth_callback=ms-appx-web:///Assets/Test.html&client_id=1d912cae47144fa09d88&md=com.huaban.android" },
			new SNSType { strName = "人人", strType = "renren", Icon = "ms-appx:///Assets/renren_50px.png", Url = "https://huaban.com/oauth/renren/?auth_callback=ms-appx-web:///Assets/Test.html&client_id=1d912cae47144fa09d88&md=com.huaban.android" }
		});
		private string _UserName;
		public string UserName
		{
			set { SetValue(ref _UserName, value); }
			get { return _UserName; }
		}
		private string _Password;
		public string Password
		{
			set { SetValue(ref _Password, value); }
			get { return _Password; }
		}

		private int _PivotIndex;
		public int PivotIndex
		{
			set { SetValue(ref _PivotIndex, value); }
			get { return _PivotIndex; }
		}

		private Visibility _WebViewVisibility;
		public Visibility WebViewVisibility
		{
			set { SetValue(ref _WebViewVisibility, value); }
			get { return _WebViewVisibility; }
		}
		private string _LoginUri;
		public string LoginUri
		{
			set { SetValue(ref _LoginUri, value); }
			get { return _LoginUri; }
		}

		public ElementTheme Theme { set; get; } = Setting.Current.DarkMode ? ElementTheme.Dark : ElementTheme.Light;
		#endregion

		#region Commands

		private DelegateCommand _LoginCommand;
		public DelegateCommand LoginCommand
		{
			get
			{
				return _LoginCommand ?? (_LoginCommand = new DelegateCommand(
					async o =>
					{
						if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
							return;
						IsLoading = true;
						try
						{
							var token = await API.Current().OAuthorAPI.GetToken(UserName, Password);
							await SetToken(token);
						}
						catch (Exception ex)
						{
							string a = ex.Message;
						}
						finally
						{
							IsLoading = false;
						}
					},
					(Object obj) => !IsLoading)
				);
			}
		}

		private DelegateCommand _CancelCommand;
		public DelegateCommand CancelCommand
		{
			get
			{
				return _CancelCommand ?? (_CancelCommand = new DelegateCommand(
					o =>
					{
						Hide();
					},
					o => true)
				);
			}
		}

		private DelegateCommand _SetPivotIndexCommand;
		public DelegateCommand SetPivotIndexCommand
		{
			get
			{
				return _SetPivotIndexCommand ?? (_SetPivotIndexCommand = new DelegateCommand(
					o =>
					{

						int idx = 0;
						int.TryParse(o.ToString(), out idx);
						PivotIndex = idx;
					},
					o => true)
				);
			}
		}

		private DelegateCommand _ShowWebViewCommand;
		public DelegateCommand ShowWebViewCommand
		{
			get
			{
				return _ShowWebViewCommand ?? (_ShowWebViewCommand = new DelegateCommand(
					o =>
					{
						var args = o as SelectionChangedEventArgs;
						var item = o as SNSType;
						if (args == null && item == null)
							return;

						if (args != null && args.AddedItems.Count > 0)
							item = args.AddedItems[0] as SNSType;

						if (item == null)
							return;

						LoginUri = item.Url;
						WebViewVisibility = Visibility.Visible;
					},
					o => true)
				);
			}
		}

		private DelegateCommand _ScriptNotifyCommand;
		public DelegateCommand ScriptNotifyCommand
		{
			get
			{
				return _ScriptNotifyCommand ?? (_ScriptNotifyCommand = new DelegateCommand(
					async o =>
					{
						var args = o as NotifyEventArgs;
						string href = args?.Value;


						if (href?.IndexOf("assets/test.html#", StringComparison.CurrentCultureIgnoreCase) > 0)
						{
							var token = AuthToken.Parse(href, true);
							await SetToken(token);
						}

					},
					o => true)
				);
			}
		}

		#endregion

		#region Methods

		private async Task SetToken(AuthToken token)
		{
			if (token.ExpiresIn > DateTime.Now)
			{
				await Context.SetToken(token);
				Save();
				SuccessAction?.Invoke(token);
				this.Hide();
			}
		}

		public void Hide()
		{
			this.Context.NavigationService.BackEvent -= NavigationService_BackEvent;
			this.Context.NavigationService.DisplayBackButton();
			this.Popup.IsOpen = false;
		}
		public void Show()
		{
			this.Popup.IsOpen = true;
			SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
			this.Context.NavigationService.BackEvent += NavigationService_BackEvent;
		}

		private void NavigationService_BackEvent(object sender, Windows.UI.Core.BackRequestedEventArgs e)
		{
			if (!e.Handled)
			{
				e.Handled = true;
				Hide();
			}
		}
		private void Save()
		{
			StorageHelper.SaveSetting("username", UserName);
			StorageHelper.SaveSetting("password", Password);
		}
		#endregion
	}
}
