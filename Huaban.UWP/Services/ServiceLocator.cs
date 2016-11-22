using System;
using Microsoft.Practices.Unity;

namespace Huaban.UWP.Services
{
	using Models;
	using Api;
	using Base;
	public class ServiceLocator
	{
		private static ServiceLocator Current { get; set; }

		public IUnityContainer Container { set; get; }
		private ServiceLocator()
		{
			this.Container = new UnityContainer();

			RegisterViewModels();
		}
		private void RegisterViewModels()
		{
			var client = new Client()
			{
				ClientID = "1d912cae47144fa09d88",
				ClientSecret = "f94fcc09b59b4349a148a203ab2f20c7",
				OAuthCallback = "ms-appx-web:///Assets/Test.html",
				MD = "com.huaban.android"
			};
			var context = new Context();
			context.NavigationService = new NavigationService(context);

			Container.RegisterInstance(context);
			Container.RegisterInstance(context.NavigationService);
			Container.RegisterInstance<IClient>(client);
			
			Container.RegisterType<BoardAPI>();
			Container.RegisterType<CategoryAPI>();
			Container.RegisterType<OAuthorAPI>();
			Container.RegisterType<PinAPI>();
			Container.RegisterType<UserAPI>();
		}
		public static ServiceLocator BuildLocator()
		{
			if (Current != null)
				return Current;

			Current = new ServiceLocator();

			return Current;
		}
		public static void RegisterInstance<T>(T value)
		{
			Current.Container.RegisterInstance(value);
		}
		public static void RegisterType<T>()
		{
			Current.Container.RegisterType<T>();
		}
		public static T Resolve<T>()
		{
			return Current.Container.Resolve<T>();
		}
		public static object Resolve(Type type)
		{
			return Current.Container.Resolve(type);
		}
	}
}
