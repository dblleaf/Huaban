using iHuaban.App.Models;
using iHuaban.App.Services;
using iHuaban.Core.Commands;
using iHuaban.Core.Models;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace iHuaban.App.ViewModels
{
    public class LoginViewModel : HBPageViewModel
    {
        private IAccountService authService;
        private IStorageService storageService;
        private Context context;
        public LoginViewModel
        (
            IAccountService authService,
            IStorageService storageService,
            Context context
        ) : base(context)
        {
            this.authService = authService;
            this.storageService = storageService;
            this.context = context;
            this.UserName = "okokit@126.com";
            this.Password = "999999999";
        }

        public async Task InitAsync(NavigationEventArgs e)
        {
            await new MessageDialog("message").ShowAsync();
            this.UserName = storageService.GetSetting(nameof(UserName));
            this.Password = storageService.GetSetting(nameof(Password));
            await Task.Delay(0);
        }

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
                            IsLoading = true;
                            var dispatcher = Window.Current.Dispatcher;
                            await Task.Run(async () =>
                            {
                                var auth = await authService.LoginAsync(UserName, Password);

                                await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                {
                                    if (!string.IsNullOrWhiteSpace(auth?.User?.user_id))
                                    {
                                        this.context.User = auth.User;

                                        this.storageService.SaveSetting(nameof(UserName), UserName);
                                        this.storageService.SaveSetting(nameof(Password), Password);
                                        this.storageService.SaveSetting("cookie", context.CookieString);
                                    }
                                    if (!string.IsNullOrWhiteSpace(auth.msg))
                                    {
                                        this.context.ShowMessage(auth.msg);
                                    }

                                    IsLoading = false;
                                });

                            });

                        }
                        catch (Exception ex)
                        {
                            this.context.ShowMessage(ex.Message);
                        }
                        await Task.Delay(0);
                    },
                    o =>
                    {
                        return IsLoading || (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password));
                    })
                );
            }
        }
    }
}
