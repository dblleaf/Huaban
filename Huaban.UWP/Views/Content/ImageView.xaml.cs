using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Huaban.UWP.Views
{
	using Controls;
	using ViewModels;
	public sealed partial class ImageView : HBControl
	{
		public ImageView()
		{
			this.InitializeComponent();

		}
		public HBViewModel HBVM
		{
			get { return this.ViewModel as HBViewModel; }
		}
	}
}
