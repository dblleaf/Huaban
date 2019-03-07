using iHuaban.App.TemplateSelectors;
using iHuaban.Core.Helpers;
using iHuaban.Core.Models;
using System;
using Unity;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.Services
{
    public class Locator
    {
        private static Locator Instance { get; set; }

        private IUnityContainer Container { set; get; }

        private Locator()
        {
            this.Container = new UnityContainer();
        }

        public static Locator BuildLocator()
        {
            if (Instance != null)
                return Instance;

            Instance = new Locator();
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
            Container.RegisterType<IHttpHelper, HttpHelper>();
            Container.RegisterSingleton<INavigationService, NavigationService>();
            Container.RegisterType<IServiceProvider, ServiceProvider>();
            Container.RegisterType<IPinService, PinService>();
            Container.RegisterType<IBoardService, BoardService>();
            Container.RegisterType<ICategoryService, CategoryService>();
            Container.RegisterType<IHomeService, HomeService>();
            Container.RegisterType<DataTemplateSelector, SupperDataTemplateSelector>();
        }

        public static T ResolveObject<T>()
        {
            return BuildLocator().Resolve<T>();
        }

        public static T ResolveObject<T>(Type type) where T : new()
        {
            return (T)BuildLocator().Resolve(type);
        }
    }
}
