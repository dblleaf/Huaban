using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Huaban.UWP
{
    using Base;
    using Models;
    using Newtonsoft.Json;
    using Services;

    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();

            this.Suspending += OnSuspending;
        }

        private async Task LoadData()
        {
            ServiceLocator.BuildLocator();

            var context = ServiceLocator.Resolve<Context>();
            var user = await StorageHelper.ReadLocal(o => JsonConvert.DeserializeObject<User>(o));
            var token = await StorageHelper.ReadLocal(o => JsonConvert.DeserializeObject<AuthToken>(o));
            if (token != null)
            {
                token = await ServiceLocator.Resolve<OAuthorService>().RefreshToken(token);
            }
            context.User = user;

            if (token != null)
            {
                if (token.ExpiresIn > DateTime.Now)
                {
                    await context.SetToken(token);
                }
                else
                {
                    token = await ServiceLocator.Resolve<OAuthorService>().RefreshToken(token);
                }
            }
        }

        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            await LoadData();

            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(Views.ShellView), e.Arguments);
                }

                Window.Current.Activate();
            }
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }
    }
}
