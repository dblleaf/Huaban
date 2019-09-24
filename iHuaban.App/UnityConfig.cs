using iHuaban.App.Helpers;
using iHuaban.App.Models;
using iHuaban.App.Services;
using iHuaban.Core.Converters;
using iHuaban.Core.Models;
using System;
using Unity;
using Windows.UI.Xaml.Data;

namespace iHuaban.App
{
    public class UnityConfig
    {
        private static UnityConfig Instance { get; set; }

        private IUnityContainer Container { set; get; }

        private UnityConfig()
        {
            this.Container = new UnityContainer();
        }

        public static UnityConfig Build()
        {
            if (Instance != null)
                return Instance;

            Instance = new UnityConfig();
            Instance.Register();
            return Instance;
        }

        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return Container.Resolve(type);
        }

        private void Register()
        {
            Container.RegisterInstance(Setting.Instance());
            Container.RegisterSingleton<Context>();
            Container.RegisterType<IApiHttpHelper, ApiHttpHelper>();
            Container.RegisterType<IAuthHttpHelper, AuthHttpHelper>();

            Container.RegisterType<IStorageService, StorageService>();
            Container.RegisterType<IThemeService, ThemeService>();
            Container.RegisterType<IValueConverter, ObjectToVisibilityConverter>();
            Container.RegisterSingleton<INavigationService, NavigationService>();
            Container.RegisterType<IService, Service>();
            Container.RegisterType<IHomeService, HomeService>();
            Container.RegisterType<IAccountService, AccountService>();
        }

        public static T ResolveObject<T>()
        {
            return Build().Resolve<T>();
        }

        public static T ResolveObject<T>(Type type)
        {
            return (T)Build().Resolve(type);
        }
    }
}
