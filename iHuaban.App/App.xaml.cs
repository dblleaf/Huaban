using iHuaban.App.Services;
using iHuaban.App.Views;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace iHuaban.App
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                }
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                await Locator.ResolveObject<IAuthService>().LoadMeAsync();
                if (rootFrame.Content == null)
                {
                    Locator.ResolveObject<INavigationService>().Navigate<ShellPage>(e.Arguments);
                }
                Window.Current.Activate();
                ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(256, 500));
                Locator.ResolveObject<IThemeService>().LoadTheme();
            }
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            Locator.BuildLocator().Resolve<IThemeService>().LoadTheme();
            base.OnActivated(args);
        }
    }
}
