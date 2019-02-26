using iHuaban.App.Models;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHuaban.Core;
using iHuaban.App.Services;
using iHuaban.Core.Helpers;
using Windows.UI.Xaml;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using iHuaban.Core.Commands;
using Windows.UI.Xaml.Controls;
using iHuaban.App.Views;

namespace iHuaban.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private INavigationService navigationService;
        public MainViewModel(INavigationService navigationService)
        {
            this.Setting.DarkMode = false;
            this.navigationService = navigationService;
            this.Setting.PropertyChanged += Setting_PropertyChanged;
        }

        private void Setting_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "WindowActived" || e.PropertyName == "DarkMode")
            {
                ExtendAcrylicIntoTitleBar();
            }
        }

        private HttpHelper httpHelper => new HttpHelper();
        public ObservableCollection<Menu> Menu => new ObservableCollection<Menu>
        {
            new Menu
            {
                Title = Constants.TextPhone,
                Icon = "\uE8EA",
                Template = Constants.TemplateGrid,
                CellMinWidth = 236,
                ScaleSize = "750:1334",
                ItemTemplateName = Constants.TemplatePhone,
                ViewModelType = typeof(PhoneViewModel),
            },
            new Menu
            {
                Title = Constants.TextPC,
                Icon = "\uE977",
                Template = Constants.TemplateGrid,
                CellMinWidth = 256,
                ScaleSize = "1920:1200",
                ItemTemplateName = Constants.TemplatePC,
                ViewModelType = typeof(PCViewModel),
            },
            new Menu
            {
                Title = Constants.TextFind,
                Icon = "\uE721",
                CellMinWidth = 236,
                Template = Constants.TemplateFind,
                ItemTemplateName = Constants.TemplateCategory,
                ViewModelType = typeof(FindViewModel),
            },
            new Menu
            {
                Title = Constants.TextMine,
                Icon = "\uE77B",
                Template = Constants.TemplateMine,
                ViewModelType = typeof(MineViewModel),
            }
        };

        public override async Task InitAsync()
        {
            this.Setting.RequestedTheme = ElementTheme.Light;
            ExtendAcrylicIntoTitleBar();
            await Task.FromResult(0);
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
