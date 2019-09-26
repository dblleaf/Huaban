using iHuaban.App.Models;
using iHuaban.App.Services;
using iHuaban.App.Views;
using iHuaban.Core.Commands;
using iHuaban.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using Windows.ApplicationModel;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace iHuaban.App.ViewModels
{
    public class ShellViewModel : PageViewModel
    {
        private INavigationService navigationService;
        private IStorageService storageService;
        private IAccountService authService;
        public ObservableCollection<MenuItem> Menu { get; set; }
        public Context Context { get; private set; }
        public string AppName { get; private set; } = Package.Current.DisplayName;
        public ShellViewModel(
            INavigationService navigationService,
            IStorageService storageService,
            IAccountService authService,
            Context context)
        {
            this.navigationService = navigationService;
            this.storageService = storageService;
            this.authService = authService;
            this.Context = context;
            var list = new List<MenuItem>
            {
                new MenuItem
                {
                    Title = Constants.TextHome,
                    Icon = Constants.IconHome,
                    Type = typeof(HomePage)
                },
                new MenuItem
                {
                    Title = Constants.TextCategory,
                    Icon = Constants.IconCategory,
                    Type = typeof(CategoriesPage)
                },
                new MenuItem
                {
                    Title = Constants.TextFind,
                    Icon = Constants.IconFind,
                    Type = typeof(FindPage)
                },
                new MenuItem
                {
                    Title = Constants.TextMine,
                    Icon = Constants.IconMine,
                    Type = typeof(MinePage)
                },
                new MenuItem
                {
                    Title = Constants.TextDownload,
                    Icon = Constants.IconDownload,
                    Type = typeof(DownloadPage)
                },
                 new MenuItem
                {
                    Title = Constants.TextSetting,
                    Icon = Constants.IconSetting,
                    Type = typeof(SettingPage)
                },
            };
            Menu = new ObservableCollection<MenuItem>(list);
            BoardPickerVisible = Visibility.Collapsed;

            this.Context.PickPinHandlder += pin =>
            {
                this.BoardPickerVisible = Visibility.Visible;
            };

            this.Context.ShowMessageHandler += async msg =>
            {
                await new MessageDialog(msg).ShowAsync();
            };
        }

        public override async void Init()
        {
            base.Init();
            string cookieJson = storageService.GetSetting("Cookies");
            var cookies = JsonConvert.DeserializeObject<List<Cookie>>(cookieJson);
            this.Context.SetCookie(cookies);
            var user = await authService.GetMeAsync();
            if (!string.IsNullOrWhiteSpace(user?.user_id))
            {
                Context.User = user;
                var lastboards = await authService.GetLastBoardsAsync();
                if (lastboards?.Boards?.Count > 0)
                {
                    this.Context.QuickBoard = lastboards.Boards[0];
                }
            }
        }
        private MenuItem _SelectedMenu;
        public MenuItem SelectedMenu
        {
            get { return _SelectedMenu; }
            set { SetValue(ref _SelectedMenu, value); }
        }

        private Visibility _BoardPickerVisible;
        public Visibility BoardPickerVisible
        {
            get { return _BoardPickerVisible; }
            set { SetValue(ref _BoardPickerVisible, value); }
        }

        private IncrementalLoadingList<Board> _BoardList;
        public IncrementalLoadingList<Board> BoardList
        {
            get { return _BoardList; }
            set { SetValue(ref _BoardList, value); }
        }

        private DelegateCommand _NavigateCommand;
        public DelegateCommand NavigateCommand
        {
            get
            {
                return _NavigateCommand ?? (_NavigateCommand = new DelegateCommand(
                o =>
                {
                    try
                    {
                        if (SelectedMenu != null)
                        {
                            navigationService.Navigate(SelectedMenu.Type);
                        }
                    }
                    catch (Exception)
                    {

                    }

                }, o => true));
            }
        }
    }
}
