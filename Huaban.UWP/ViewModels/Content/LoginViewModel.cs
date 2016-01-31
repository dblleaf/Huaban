using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Huaban.UWP.ViewModels
{
	using UWP.Commands;
	using Views;
	using Base;
	using Models;

	public class LoginViewModel : HBViewModel
	{
		protected Popup Popup { set; get; }
		private Action<AuthToken> SuccessAction;
		public LoginViewModel(Context context, Action<AuthToken> successAction)
			: base(context)
		{
			Title = "登陆";
			LeftHeaderVisibility = Windows.UI.Xaml.Visibility.Collapsed;
			SuccessAction = successAction;

			UserName = StorageHelper.GetSetting("username");
			Password = StorageHelper.GetSetting("password");

			Popup = new Popup();
			Popup.Child = new LoginView();
			Popup.DataContext = this;

			Loading += () =>
			{
				_LoginCommand.RaiseCanExecuteChanged();
			};
		}
		private string _UserName;
		public string UserName
		{
			set
			{
				SetValue(ref _UserName, value);
			}
			get { return _UserName; }
		}
		private string _Password;
		public string Password
		{
			set
			{
				SetValue(ref _Password, value);
			}
			get { return _Password; }
		}
		public ElementTheme Theme { set; get; } = Setting.Current.DarkMode ? ElementTheme.Dark : ElementTheme.Light;
		private DelegateCommand _LoginCommand;
		public DelegateCommand LoginCommand
		{
			get
			{
				return _LoginCommand ?? (_LoginCommand = new DelegateCommand(
					async o =>
					{
						IsLoading = true;
						try
						{
							var token = await API.Current().OAuthorAPI.GetToken(UserName, Password);
							if (token.ExpiresIn > DateTime.Now)
							{
								await Context.SetToken(token);
								Save();
								SuccessAction?.Invoke(token);
								this.Popup.IsOpen = false;
							}
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
	}
}
