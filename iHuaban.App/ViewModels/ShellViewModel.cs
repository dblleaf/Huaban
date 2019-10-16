using iHuaban.App.Helpers;
using iHuaban.App.Models;
using iHuaban.App.Services;
using iHuaban.App.Views;
using iHuaban.App.Views.Content;
using iHuaban.Core.Commands;
using iHuaban.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace iHuaban.App.ViewModels
{
    public class ShellViewModel : PageViewModel
    {
        private INavigationService navigationService;
        private IStorageService storageService;
        private IAccountService authService;
        private IApiHttpHelper httpHelper;
        public ObservableCollection<MenuItem> Menu { get; set; }
        public Context Context { get; private set; }
        public string AppName { get; private set; } = Package.Current.DisplayName;
        public ShellViewModel(
            INavigationService navigationService,
            IStorageService storageService,
            IAccountService authService,
            IApiHttpHelper httpHelper,
            Context context)
        {
            this.navigationService = navigationService;
            this.storageService = storageService;
            this.authService = authService;
            this.httpHelper = httpHelper;
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

            this.Context.ShowMessageHandler += msg =>
           {
               PopupMessage.ShowMessage(msg);
           };
        }

        public override async void Init()
        {
            base.Init();
            SelectedMenu = Menu[0];
            await this.InitPath();
            await this.InitUser();
        }

        private async Task InitUser()
        {
            string cookieJson = storageService.GetSetting("Cookies");
            var cookies = JsonConvert.DeserializeObject<List<Cookie>>(cookieJson);
            this.Context.SetCookie(cookies);
            var dispatcher = Window.Current.Dispatcher;
            await Task.WhenAll(
                Task.Run(async () =>
                {
                    var user = await authService.GetMeAsync();
                    if (!string.IsNullOrWhiteSpace(user?.user_id))
                    {
                        await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            this.Context.User = user;
                        });

                    }
                }),
                Task.Run(async () =>
                {
                    var boardCollection = await authService.GetLastBoardsAsync();
                    if (boardCollection?.Boards?.Count > 0)
                    {
                        await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            this.Context.QuickBoard = boardCollection.Boards[0];
                        });

                    }
                })
            ).ContinueWith(p => { });
        }
        private async Task InitPath()
        {
            var savePath = storageService.GetSetting("SavePath");

            if (!string.IsNullOrWhiteSpace(savePath))
            {
                try
                {
                    await StorageFolder.GetFolderFromPathAsync(savePath);
                }
                catch (Exception)
                {
                    await KnownFolders.PicturesLibrary.CreateFolderAsync("huaban", CreationCollisionOption.OpenIfExists);
                }
            }
            else
            {
                await KnownFolders.PicturesLibrary.CreateFolderAsync("huaban", CreationCollisionOption.OpenIfExists);
            }
        }
        private MenuItem _SelectedMenu;
        public MenuItem SelectedMenu
        {
            get { return _SelectedMenu; }
            set { SetValue(ref _SelectedMenu, value); }
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
