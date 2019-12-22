using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Huaban.UWP.ViewModels
{
    using Base;
    using Commands;
    using Models;
    using Services;
    using Views;
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
                            var token = await Services.ServiceLocator.Resolve<OAuthorService>().GetToken(UserName, Password);
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
            NavigationService.BackEvent -= NavigationService_BackEvent;
            NavigationService.DisplayBackButton();
            this.Popup.IsOpen = false;
        }
        public void Show()
        {
            this.Popup.IsOpen = true;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            NavigationService.BackEvent += NavigationService_BackEvent;
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
