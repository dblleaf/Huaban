using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using Windows.Foundation.Metadata;

namespace Huaban.UWP.ViewModels
{
    using Base;
    using Controls;
    using Commands;
    using Models;
    using Services;
    public class SettingViewModel : HBViewModel
    {
        public SettingViewModel(Context context)
            : base(context)
        {
            Title = "设置";
            Setting = Setting.Current;

            if (!ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                PCVisibility = Visibility.Visible;
            }
            else
            {
                PCVisibility = Visibility.Collapsed;
            }
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

        private Visibility _PCVisibility;
        public Visibility PCVisibility
        {
            get { return _PCVisibility; }
            set { SetValue(ref _PCVisibility, value); }
        }
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
                        try
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
                        }
                        catch (Exception ex)
                        {

                        }

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
                        try
                        {
                            await Context.ClearToken();
                            Context.ShowTip("已经退出");
                            LogoutCommand.RaiseCanExecuteChanged();
                        }
                        catch (Exception ex)
                        {

                        }

                    },
                    o => Context.IsLogin)
                );
            }
        }

        private DelegateCommand _SelectePathCommand;
        public DelegateCommand SelectePathCommand
        {
            get
            {
                return _SelectePathCommand ?? (_SelectePathCommand = new DelegateCommand(async o =>
                {
                    try
                    {
                        FolderPicker fp = new FolderPicker();
                        fp.FileTypeFilter.Add(".jpg");
                        fp.ViewMode = PickerViewMode.List;
                        fp.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                        var folder = await fp.PickSingleFolderAsync();

                        if (folder != null)
                        {
                            StorageApplicationPermissions.FutureAccessList.Add(folder);
                            this.Setting.SavePath = folder;
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }, o => true));
            }
        }

        private DelegateCommand _OpenPathCommand;
        public DelegateCommand OpenPathCommand
        {
            get
            {
                return _OpenPathCommand ?? (_OpenPathCommand = new DelegateCommand(async o =>
                {
                    try
                    {
                        await Windows.System.Launcher.LaunchFolderAsync(Setting.SavePath);
                    }
                    catch (Exception ex)
                    {

                    }

                }, o => true));
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
