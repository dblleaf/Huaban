using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.Services
{
    public class NavigationService : INavigationService
    {
        private Frame rootFrame;
        public NavigationService()
        {
            rootFrame = Window.Current.Content as Frame;
            SystemNavigationManager.GetForCurrentView().BackRequested += NavigationService_BackRequested;
        }

        private void NavigationService_BackRequested(object sender, BackRequestedEventArgs e)
        {
            bool handled = e.Handled;

            if (!e.Handled)
                this.BackRequested(ref handled);

            e.Handled = handled;
        }

        private bool b;
        private void BackRequested(ref bool handled)
        {
            if (this.rootFrame.CanGoBack)
            {
                handled = true;
                this.rootFrame.GoBack();
                DisplayBackButton();
            }
            else if (!this.rootFrame.CanGoBack && !handled)
            {
                if (b)
                {
                    App.Current.Exit();
                }
                else
                {
                    b = true;
                    handled = true;
                    Task.Run(async () =>
                    {
                        await Task.Delay(1500);
                        b = false;
                    });
                }
            }
        }

        public void Navigate(string pageName)
        {
            var pageType = GetPageType(pageName);
            Navigate(pageType);
        }

        public void Navigate(Type pageType)
        {
            if (pageType != null && typeof(Page).IsAssignableFrom(pageType))
            {
                rootFrame.Navigate(pageType);
                DisplayBackButton();
            }
        }

        public void Navigate<T>()
        {
            Navigate(typeof(T));
        }

        public void DisplayBackButton()
        {
            if (rootFrame.CanGoBack)
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            else
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private Type GetPageType(string pageName)
        {
            var type = this.GetType();
            var assemblyQualifiedAppType = type.AssemblyQualifiedName;

            var pageNameWithParameter = assemblyQualifiedAppType.Replace(type.FullName, type.Namespace.Substring(0, type.Namespace.LastIndexOf('.')) + ".Views.{0}Page");

            var viewFullName = string.Format(pageNameWithParameter, pageName);
            var viewType = Type.GetType(viewFullName);

            if (viewType == null)
            {
                throw new ArgumentException(
                    "Invalid page type",
                    "pageToken");
            }

            return viewType;
        }
    }
}
