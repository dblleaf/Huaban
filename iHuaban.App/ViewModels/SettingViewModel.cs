using iHuaban.App.Services;
using iHuaban.Core.Commands;
using iHuaban.Core.Models;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;

namespace iHuaban.App.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        private IThemeService themeService;
        private IStorageService storageService;
        public SettingViewModel(IThemeService themeService, IStorageService storageService)
        {
            this.themeService = themeService;
            this.storageService = storageService;
            this.DarkMode = themeService.RequestTheme == ElementTheme.Dark;
            this.PropertyChanged += SettingViewModel_PropertyChanged;
        }

        private bool _DarkMode;
        public bool DarkMode
        {
            get { return _DarkMode; }
            set { SetValue(ref _DarkMode, value); }
        }

        private StorageFolder _SavePath;
        public StorageFolder SavePath
        {
            get { return _SavePath; }
            set { SetValue(ref _SavePath, value); }
        }

        private DelegateCommand logoutCommand;
        public DelegateCommand LogoutCommand
        {
            get
            {
                return logoutCommand ?? (logoutCommand = new DelegateCommand(
                    async o =>
                    {
                        await Task.Delay(0);
                    },
                    o => true)
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
                    FolderPicker fp = new FolderPicker();
                    fp.FileTypeFilter.Add(".jpg");
                    fp.ViewMode = PickerViewMode.List;
                    fp.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                    var folder = await fp.PickSingleFolderAsync();

                    if (folder != null)
                    {
                        StorageApplicationPermissions.FutureAccessList.Add(folder);
                        this.SavePath = folder;
                    }
                }, o => true));
            }
        }

        public override async Task InitAsync()
        {
            await Task.Delay(0);
            var savePath = storageService.GetSetting("SavePath");

            if (!string.IsNullOrWhiteSpace(savePath))
            {
                try
                {
                    SavePath = await StorageFolder.GetFolderFromPathAsync(savePath);
                }
                catch (Exception ex)
                {
                    SavePath = await KnownFolders.PicturesLibrary.CreateFolderAsync("huaban", CreationCollisionOption.OpenIfExists);
                }
            }
            else
            {
                SavePath = await KnownFolders.PicturesLibrary.CreateFolderAsync("huaban", CreationCollisionOption.OpenIfExists);
            }
        }

        private void SettingViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DarkMode))
            {
                ElementTheme theme = DarkMode ? ElementTheme.Dark : ElementTheme.Light;
                this.themeService.SetTheme(theme);
            }
            else if (e.PropertyName == nameof(SavePath))
            {
                storageService.SaveSetting(nameof(SavePath), SavePath.Path);
            }
        }
    }
}
