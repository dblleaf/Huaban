using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;

namespace Huaban.UWP.Views
{
	using Controls;
	using Services;
	using Base;
	public sealed partial class ShellView : HBPage
	{
		public ShellView()
		{
			this.InitializeComponent();
			var context = ServiceLocator.Resolve<Context>();
			if (context != null)
			{
				context.SetDispatcher( this.Dispatcher);
				context.NavigationService.SetFrame(MainFrame, DetailFrame);
			}

		}
	}
}
