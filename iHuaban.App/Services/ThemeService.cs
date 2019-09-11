using System;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace iHuaban.App.Services
{
    public class ThemeService : IThemeService
    {
        class Constants
        {
            internal const string RequestTheme = "RequestTheme";
        }

        public static ElementTheme Theme { get; set; } = ElementTheme.Dark;
        public ElementTheme RequestTheme => ThemeService.Theme;
        private IStorageService StorageService { get; set; }
        public ThemeService(IStorageService storageService)
        {
            this.StorageService = storageService;
        }

        public void SetTheme(ElementTheme theme)
        {
            ThemeService.Theme = theme;
            SetTheme();
        }

        public void LoadTheme()
        {
            string themeStr = StorageService.GetSetting(Constants.RequestTheme);
            if (Enum.TryParse<ElementTheme>(themeStr, out ElementTheme theme))
            {
                Theme = theme;
                SetTheme();
            }
        }

        public void SetTheme()
        {
            if (Window.Current.Content is FrameworkElement frameworkElement)
            {
                frameworkElement.RequestedTheme = Theme;
            }
            SetTitleBar();
            StorageService.SaveSetting(Constants.RequestTheme, Theme.ToString());
        }

        private void SetTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            Color color = Colors.White;
            Color bgColor = Color.FromArgb(255, 50, 50, 50);
            if (ThemeService.Theme == ElementTheme.Light)
            {
                color = Colors.Black;
                bgColor = Color.FromArgb(255, 205, 205, 205);
            }
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Transparent;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = color;
            titleBar.ButtonInactiveForegroundColor = color;
            titleBar.ButtonHoverBackgroundColor = bgColor;
            titleBar.ButtonHoverForegroundColor = color;
        }
    }
}
