﻿using iHuaban.App.Models;
using System;
using Unity;

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
        }
    }
}
