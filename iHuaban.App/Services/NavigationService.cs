using iHuaban.Core;
using iHuaban.Core.Models;
using System;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace iHuaban.App.Services
{
    public class NavigationService : INavigationService
    {
        private static Frame rootFrame;
        private static Frame splitViewFrame;

        static NavigationService()
        {
            NavigationService.rootFrame = Window.Current.Content as Frame;
            NavigationService.rootFrame.Navigated += RootFrame_Navigated;
        }

        public static void SetSplitViewFrame(Frame splitViewFrame)
        {
            NavigationService.splitViewFrame = splitViewFrame;
            NavigationService.splitViewFrame.Navigated += SplitViewFrame_Navigated;
        }

        private static void SplitViewFrame_Navigated(object sender, NavigationEventArgs e)
        {
            var interfaces = e.SourcePageType.GetInterfaces();
            if (e.Content is Page page)
            {
                foreach (var it in interfaces)
                {
                    var itInfo = it.GetTypeInfo();

                    if (itInfo.IsGenericType && itInfo.GetGenericTypeDefinition() == typeof(IView<>))
                    {
                        foreach (var genericType in itInfo.GenericTypeArguments)
                        {
                            if (typeof(PageViewModel).IsAssignableFrom(genericType))
                            {
                                var vm = UnityConfig.ResolveObject<PageViewModel>(genericType);
                                page.DataContext = vm;
                                return;
                            }
                        }
                    }
                }
            }
        }

        private static void RootFrame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            var interfaces = e.SourcePageType.GetInterfaces();
            if (e.Content is Page page)
            {
                if (page.DataContext != null)
                {
                    return;
                }
                foreach (var it in interfaces)
                {
                    var itInfo = it.GetTypeInfo();

                    if (itInfo.IsGenericType && itInfo.GetGenericTypeDefinition() == typeof(IPage<>))
                    {
                        foreach (var genericType in itInfo.GenericTypeArguments)
                        {
                            if (typeof(PageViewModel).IsAssignableFrom(genericType))
                            {
                                var vm = UnityConfig.ResolveObject<PageViewModel>(genericType);
                                page.DataContext = vm;
                                return;
                            }
                        }
                    }
                }
            }
        }

        public void Navigate(string pageName, object parameter = null)
        {
            var pageType = GetPageType(pageName);
            Navigate(pageType, parameter);
        }

        public void Navigate(Type pageType, object parameter = null)
        {
            if (pageType != null && pageType.GetInterfaces().Any(o => o.IsGenericType && o.GetGenericTypeDefinition() == typeof(IView<>)))
            {
                splitViewFrame.Navigate(pageType, parameter, new SuppressNavigationTransitionInfo());
                //DisplayBackButton();
            }
            else if (pageType != null && pageType.GetInterfaces().Any(o => o.IsGenericType && o.GetGenericTypeDefinition() == typeof(IPage<>)))
            {
                rootFrame.Navigate(pageType, parameter, new SuppressNavigationTransitionInfo());
            }
        }

        public void Navigate<T>(object parameter = null)
        {
            Navigate(typeof(T));
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
