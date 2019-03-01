using iHuaban.App.Models;
using iHuaban.App.Services;
using iHuaban.App.TemplateSelectors;
using iHuaban.Core.Commands;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private INavigationService navigationService;
        private bool IsInit = false;
        public DataTemplateSelector DataTemplateSelector { get; private set; }
        private ObservableCollection<Menu> _Menu;
        public ObservableCollection<Menu> Menu
        {
            get { return _Menu; }
            set { SetValue(ref _Menu, value); }
        }
        public MainViewModel(INavigationService navigationService, DataTemplateSelector dataTemplateSelector)
        {
            this.Setting.DarkMode = false;
            this.navigationService = navigationService;
            this.DataTemplateSelector = dataTemplateSelector;
            this.Setting.PropertyChanged += Setting_PropertyChanged;
        }

        private void Setting_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "WindowActived" || e.PropertyName == "DarkMode")
            {
                ExtendAcrylicIntoTitleBar();
            }
        }

        public override async Task InitAsync()
        {
            if (IsInit)
                return;
            IsInit = true;

            this.Setting.RequestedTheme = ElementTheme.Light;
            ExtendAcrylicIntoTitleBar();

            var list = new List<Menu>()
            {
                new Menu
                {
                    Title = Constants.TextHome,
                    Icon = Constants.IconHome,
                    TemplateName = Constants.TemplateGrid,
                    CellMinWidth = 236,
                    ScaleSize = "300:300",
                    ItemTemplateSelector = new SupperDataTemplateSelector(),
                    ViewModelType = typeof(HomeViewModel),
                },
                new Menu
                {
                    Title = Constants.TextFind,
                    Icon = Constants.IconFind, //"\uE721",
                    CellMinWidth = 236,
                    TemplateName = Constants.TemplateFind,
                    ItemTemplateSelector = new SupperDataTemplateSelector(),
                    ViewModelType = typeof(FindViewModel),
                },
                new Menu
                {
                    Title = Constants.TextMine,
                    Icon = Constants.IconMine, //"\uE77B",
                    TemplateName = Constants.TemplateMine,
                    ViewModelType = typeof(MineViewModel),
                }
            };

            Menu = new ObservableCollection<Menu>(list);
            await Task.Delay(0);
        }

        private void ExtendAcrylicIntoTitleBar()
        {
            this.Setting.RequestedTheme = Setting.WindowActived ? (Setting.DarkMode ? ElementTheme.Dark : ElementTheme.Light) : (Application.Current.RequestedTheme == ApplicationTheme.Dark ? ElementTheme.Dark : ElementTheme.Light);
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            Color color = Colors.White;
            Color bgColor = Color.FromArgb(255, 50, 50, 50);
            if (this.Setting.RequestedTheme == ElementTheme.Light)
            {
                color = Colors.Black;
                bgColor = Color.FromArgb(255, 205, 205, 205);
            }
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Transparent;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = color;
            titleBar.ButtonInactiveForegroundColor = color;
            titleBar.ButtonHoverBackgroundColor = bgColor;
            titleBar.ButtonHoverForegroundColor = color;
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
                        string page = o.ToString();
                        navigationService.Navigate(page);
                    }
                    catch (Exception ex)
                    {

                    }

                }, o => true));
            }
        }
    }
}
