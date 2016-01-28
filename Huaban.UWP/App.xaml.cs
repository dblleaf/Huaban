using ImageLib;
using ImageLib.Cache.Memory.CacheImpl;
using ImageLib.Cache.Storage;
using ImageLib.Cache.Storage.CacheImpl;
using ImageLib.Gif;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Windows.Foundation.Metadata;
using Windows.Storage;

namespace Huaban.UWP
{
	using Base;
	using Services;
	using Models;
	/// <summary>
	/// 提供特定于应用程序的行为，以补充默认的应用程序类。
	/// </summary>
	sealed partial class App : Application
	{
		public static Context AppContext { private set; get; } = new Context();
		/// <summary>
		/// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
		/// 已执行，逻辑上等同于 main() 或 WinMain()。
		/// </summary>
		public App()
		{
			Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
				Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
				Microsoft.ApplicationInsights.WindowsCollectors.Session);
			this.InitializeComponent();
			this.InitLocator();

			this.Suspending += OnSuspending;

			ImageLoader.Initialize(new ImageConfig.Builder()
			{
				CacheMode = ImageLib.Cache.CacheMode.MemoryAndStorageCache,
				MemoryCacheImpl = new LRUMemoryCache(),
				StorageCacheImpl = new LimitedStorageCache(ApplicationData.Current.LocalCacheFolder,
				"cache", new SHA1CacheGenerator(), 1024 * 1024 * 1024)
			}.AddDecoder<GifDecoder>().Build(), true);

		}
		private void InitLocator()
		{
			ServiceLocator.BuildLocator();
			ServiceLocator.RegisterInstance(AppContext);
		}

		private void InitLayout()
		{
			if (!ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
			{

				var titleBar = ApplicationView.GetForCurrentView().TitleBar;
				var color = Color.FromArgb(255, 214, 23, 24);
				titleBar.BackgroundColor = color;
				titleBar.ButtonBackgroundColor = color;
				titleBar.ButtonForegroundColor = Colors.White;
				titleBar.ButtonHoverBackgroundColor = Colors.Red;
				titleBar.ButtonHoverForegroundColor = Colors.White;
				titleBar.ButtonInactiveBackgroundColor = color;
				titleBar.ButtonInactiveForegroundColor = Colors.White;
				titleBar.ButtonPressedBackgroundColor = color;
				titleBar.ButtonPressedForegroundColor = Colors.White;
				titleBar.ForegroundColor = Colors.White;
				titleBar.InactiveBackgroundColor = color;
				titleBar.InactiveForegroundColor = Colors.White;
			}
		}

		//加载数据
		private async Task LoadData()
		{
			var user = await StorageHelper.ReadLocal(o => SerializeExtension.JsonDeserlialize<User>(o));
			var token = await StorageHelper.ReadLocal(o => SerializeExtension.JsonDeserlialize<AuthToken>(o));
			if (token != null)
			{
				token = await AppContext.API.OAuthorAPI.RefreshToken(token);
				//UserItem.Special = true;
			}

			AppContext.User = user;
			if (token != null && token.ExpiresIn > DateTime.Now)
			{
				//UserItem.Special = true;
				await AppContext.SetToken(token);
			}
			//NotifyPropertyChanged("User");
		}

		/// <summary>
		/// 在应用程序由最终用户正常启动时进行调用。
		/// 将在启动应用程序以打开特定文件等情况下使用。
		/// </summary>
		/// <param name="e">有关启动请求和过程的详细信息。</param>
		protected async override void OnLaunched(LaunchActivatedEventArgs e)
		{
			await LoadData();

			Frame rootFrame = Window.Current.Content as Frame;

			// 不要在窗口已包含内容时重复应用程序初始化，
			// 只需确保窗口处于活动状态
			if (rootFrame == null)
			{
				// 创建要充当导航上下文的框架，并导航到第一页
				rootFrame = new Frame();

				rootFrame.NavigationFailed += OnNavigationFailed;

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					//TODO: 从之前挂起的应用程序加载状态
				}

				// 将框架放在当前窗口中
				Window.Current.Content = rootFrame;
			}

			if (rootFrame.Content == null)
			{
				// 当导航堆栈尚未还原时，导航到第一页，
				// 并通过将所需信息作为导航参数传入来配置
				// 参数
				rootFrame.Navigate(typeof(Views.ShellView), e.Arguments);
			}
			this.InitLayout();
			// 确保当前窗口处于活动状态
			Window.Current.Activate();
		}

		/// <summary>
		/// 导航到特定页失败时调用
		/// </summary>
		///<param name="sender">导航失败的框架</param>
		///<param name="e">有关导航失败的详细信息</param>
		void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
		}

		/// <summary>
		/// 在将要挂起应用程序执行时调用。  在不知道应用程序
		/// 无需知道应用程序会被终止还是会恢复，
		/// 并让内存内容保持不变。
		/// </summary>
		/// <param name="sender">挂起的请求的源。</param>
		/// <param name="e">有关挂起请求的详细信息。</param>
		private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();
			//TODO: 保存应用程序状态并停止任何后台活动
			deferral.Complete();
		}
	}
}
