using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using ImageLib;
using ImageLib.Cache.Memory.CacheImpl;
using ImageLib.Cache.Storage;
using ImageLib.Cache.Storage.CacheImpl;
using ImageLib.Gif;

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
			Current = this;
			InitImageLoader();

			this.Loaded += (s, e) =>
			{
				InitLayout();
			};
			var context = ServiceLocator.Resolve<Context>();
			if (context != null)
			{
				context.NavigationService.SetFrame(MainFrame, DetailFrame);
			}

		}
		public static ShellView Current { private set; get; }

		private void InitImageLoader()
		{
			ImageLoader.Initialize(new ImageConfig.Builder()
			{
				CacheMode = ImageLib.Cache.CacheMode.MemoryAndStorageCache,
				MemoryCacheImpl = new LRUMemoryCache(),
				StorageCacheImpl = new LimitedStorageCache(ApplicationData.Current.LocalCacheFolder,
				"cache", new SHA1CacheGenerator(), 1024 * 1024 * 1024)
			}.AddDecoder<GifDecoder>().Build(), true);
		}

		private void InitLayout()
		{
			if (!ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
			{
				var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
				coreTitleBar.ExtendViewIntoTitleBar = true;

				var titleBar = ApplicationView.GetForCurrentView().TitleBar;

				titleBar.BackgroundColor = Colors.Transparent;
				titleBar.ButtonBackgroundColor = Colors.Transparent;
				titleBar.ButtonForegroundColor = Colors.White;
				titleBar.ButtonHoverBackgroundColor = Color.FromArgb(122, 0, 0, 0);
				titleBar.ButtonHoverForegroundColor = Colors.White;
				titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
				titleBar.ButtonInactiveForegroundColor = Colors.White;
				titleBar.ButtonPressedBackgroundColor = Colors.Transparent;
				titleBar.ButtonPressedForegroundColor = Colors.White;
				titleBar.ForegroundColor = Colors.White;
				titleBar.InactiveBackgroundColor = Colors.Transparent;
				titleBar.InactiveForegroundColor = Colors.White;
			}
		}
	}
}
