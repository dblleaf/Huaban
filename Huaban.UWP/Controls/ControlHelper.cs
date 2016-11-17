using System;

namespace Huaban.UWP.Controls
{
	using Services;
	internal class ControlHelper
	{
		internal static object GetViewModel(Type type)
		{
			string pageName = type.Name;

			var assemblyQualifiedAppType = type.AssemblyQualifiedName;

			var pageNameWithParameter = assemblyQualifiedAppType.Replace(type.FullName, type.Namespace.Substring(0, type.Namespace.LastIndexOf('.')) + ".ViewModels.{0}Model");

			var viewFullName = string.Format(pageNameWithParameter, pageName);
			var viewType = Type.GetType(viewFullName);

			if (viewType == null)
			{
				return null;
			}
			var vm = ServiceLocator.Resolve(viewType);
			return vm;
		}
	}
}
