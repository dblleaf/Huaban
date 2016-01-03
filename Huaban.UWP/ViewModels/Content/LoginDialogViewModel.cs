using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Huaban.UWP.ViewModels
{
	using UWP.Commands;
	using Views;
	using Base;


	public class LoginDialogViewModel : ViewModelBase
	{
		protected ContentDialog Dialog { set; get; }
		protected bool DialogResult { set; get; }
		private Context Context { get; set; }
		public LoginDialogViewModel(Context context)
		{
			Context = context;
			//UserName = "okokit@126.com";
			Dialog = new LoginDialog();
			Dialog.DataContext = this;
			UserName = StorageHelper.GetSetting("username");
			Password = StorageHelper.GetSetting("password");

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

		private DelegateCommand _LoginCommand;
		public DelegateCommand LoginCommand
		{
			get
			{
				return _LoginCommand ?? (_LoginCommand = new DelegateCommand(
					async (Object obj) =>
					{
						IsLoading = true;
						try
						{
							var token = await API.Current().OAuthorAPI.GetToken(UserName, Password);
							if (token.ExpiresIn > DateTime.Now)
							{
								await Context.SetToken(token);
								Save();
								DialogResult = true;
								Dialog.Hide();
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
					(Object obj) =>
					{
						Dialog.Hide();
					},
					(Object obj) => true)
				);
			}
		}

		public async Task<bool> Show()
		{
			await this.Dialog.ShowAsync();
			return DialogResult;
		}

		private void Save()
		{
			StorageHelper.SaveSetting("username", UserName);
			StorageHelper.SaveSetting("password", Password);
		}
	}
}
