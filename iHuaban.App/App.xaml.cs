﻿using iHuaban.App.Services;
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
            this.UnhandledException += App_UnhandledException;
        }

        private async void App_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            await new ContentDialog
            {
                Title = "发生错误",
                Content = e.Message,
                CloseButtonText = "关闭",
                DefaultButton = ContentDialogButton.Close
            }.ShowAsync();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            UnityConfig.Build();
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
                if (rootFrame.Content == null)
                {
                    UnityConfig.ResolveObject<INavigationService>().Navigate<ShellPage>(e.Arguments);
                }
                Window.Current.Activate();
                UnityConfig.ResolveObject<IThemeService>().LoadTheme();
                ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(256, 500));
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
            base.OnActivated(args);
        }

    }
}
