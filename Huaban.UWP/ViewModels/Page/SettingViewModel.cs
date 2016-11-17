using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Huaban.UWP.ViewModels
{
	using Base;
	using Controls;
	using Commands;
	using Models;
	public class SettingViewModel : HBViewModel
	{
		public SettingViewModel(Context context)
			: base(context)
		{
			Title = "设置";
			Setting = Setting.Current;

			Setting.PropertyChanged += (s, e) =>
			{
				NotifyPropertyChanged(e.PropertyName);
			};
		}

		#region Properties
		private string _CacheSize;
		public string CacheSize
		{
			get { return _CacheSize; }
			set { SetValue(ref _CacheSize, value); }
		}

		public Setting Setting { private set; get; }

		#endregion

		#region Commands

		private DelegateCommand _ReViewCommand;
		public DelegateCommand ReViewCommand
		{
			get
			{
				return _ReViewCommand ?? (_ReViewCommand = new DelegateCommand(
					async o =>
					{
						await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://review/?ProductId=9NBLGGH5FWXP"));//9NBLGGH5PJJT//9NBLGGH5FWXP
					},
					o => !IsLoading)
				);
			}
		}

		private DelegateCommand _ClearCacheCommand;
		public DelegateCommand ClearCacheCommand
		{
			get
			{
				return _ClearCacheCommand ?? (_ClearCacheCommand = new DelegateCommand(
					async o =>
					{
						IsLoading = true;
						ClearCacheCommand.RaiseCanExecuteChanged();
						CacheSize = "清理中...";
						await Task.Delay(1500);
						await StorageHelper.ClearCache();

						await Task.Delay(500);

						CacheSize = GetFormatSize(await StorageHelper.GetCacheFolderSize());
						IsLoading = false;
						ClearCacheCommand.RaiseCanExecuteChanged();
						Context.ShowTip("清理完成");
					},
					o => !IsLoading)
				);
			}
		}

		private DelegateCommand _LogoutCommand;
		public DelegateCommand LogoutCommand
		{
			get
			{
				return _LogoutCommand ?? (_LogoutCommand = new DelegateCommand(
					async o =>
					{
						await Context.ClearToken();
						Context.ShowTip("已经退出");
						LogoutCommand.RaiseCanExecuteChanged();
					},
					o => Context.IsLogin)
				);
			}
		}

		#endregion

		#region Methods
		public async override void OnNavigatedTo(HBNavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			LogoutCommand.RaiseCanExecuteChanged();

			if (e.NavigationMode == NavigationMode.New)
			{
				CacheSize = GetFormatSize(await StorageHelper.GetCacheFolderSize());
			}
		}
		private string GetFormatSize(double size)
		{
			if (size < 1024)
			{
				return size + "byte";
			}
			else if (size < 1024 * 1024)
			{
				return Math.Round(size / 1024, 2) + "KB";
			}
			else if (size < 1024 * 1024 * 1024)
			{
				return Math.Round(size / 1024 / 1024, 2) + "MB";
			}
			else
			{
				return Math.Round(size / 1024 / 1024 / 2014, 2) + "GB";
			}
		}

		#endregion
	}
}
