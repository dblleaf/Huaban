using System;
using Microsoft.Practices.Unity;

namespace Huaban.UWP.Services
{
	public class ServiceLocator
	{
		public static ServiceLocator Current { get; private set; }

		public IUnityContainer Container { set; get; }
		private ServiceLocator()
		{
			this.Container = new UnityContainer();

			//this.Container.RegisterInstance(API.Current());
			//RegisterViewModels();
		}
		private void RegisterViewModels()
		{

		}
		public static ServiceLocator BuildLocator(NavigationService ns = null)
		{
			if (Current != null)
				return Current;

			Current = new ServiceLocator();
			//Current.Container.RegisterInstance(ns);
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
