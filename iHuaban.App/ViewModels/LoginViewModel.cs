using iHuaban.App.Models;
using iHuaban.App.Services;
using iHuaban.Core.Commands;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private IAuthService authService;
        private Context context;
        public LoginViewModel(IAuthService authService, Context context)
        {
            this.authService = authService;
            this.context = context;
            this.UserName = "okokit@126.com";
            this.Password = "999999999";
        }

        public override string TemplateName => Constants.TemplateLogin;

        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set
            {
                SetValue(ref _UserName, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                SetValue(ref _Password, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        private string _LoginUri;
        public string LoginUri
        {
            set { SetValue(ref _LoginUri, value); }
            get { return _LoginUri; }
        }

        private Visibility _WebViewVisibility;
        public Visibility WebViewVisibility
        {
            set { SetValue(ref _WebViewVisibility, value); }
            get { return _WebViewVisibility; }
        }

        private DelegateCommand _LoginCommand;
        public DelegateCommand LoginCommand
        {
            get
            {
                return _LoginCommand ?? (_LoginCommand = new DelegateCommand(
                    async o =>
                    {
                        try
                        {
                            var result = await authService.LoginAsync(UserName, Password);
                            var aaa = this.context.Cookies;
                            if (!result)
                            {
                                this.context.ShowMessage("登录失败！");
                            }

                            var user = await authService.GetMeAsync();
                            if (!string.IsNullOrWhiteSpace(user.user_id))
                            {
                                this.context.User = user;
                            }

                        }
                        catch (Exception ex)
                        {

                            this.context.ShowMessage(ex.Message);
                        }

                        await Task.Delay(0);
                    },
                    o =>
                    {
                        return !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);
                    })
                );
            }
        }

    }
}
