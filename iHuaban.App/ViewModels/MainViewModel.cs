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

namespace iHuaban.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
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
                ViewModel = new PinListViewModel<BoardCollection, Board>(new BoardService(Constants.ApiPhoneBoard, httpHelper))
            },
            new Menu
            {
                Title = Constants.TextPC,
                Icon = "\uE977",
                Template = Constants.TemplateGrid,
                CellMinWidth = 360,
                ScaleSize = "1920:1080",
                ItemTemplateName = Constants.TemplatePC,
                ViewModel = new PinListViewModel<BoardCollection, Board>(new BoardService(Constants.ApiPCBoard, httpHelper))
            },
            new Menu
            {
                Title = Constants.TextFind,
                Icon = "\uE721",
                CellMinWidth = 236,
                Template = Constants.TemplateFind,
                ItemTemplateName = Constants.TemplateCategory,
                ViewModel = new FindViewModel(new HbService<CategoryCollection>(Constants.ApiCategoriesName, httpHelper)),
            },
            new Menu
            {
                Title = Constants.TextMine,
                Icon = "\uE77B",
                Template = Constants.TemplateMine,
                ViewModel = new MineViewModel()
            }
        };

        private ElementTheme _RequestedTheme;
        public ElementTheme RequestedTheme
        {
            get { return _RequestedTheme; }
            set { SetValue(ref _RequestedTheme, value); }
        }

        public override async Task InitAsync()
        {
            ExtendAcrylicIntoTitleBar();
            await Task.FromResult(0);
        }

        private void ExtendAcrylicIntoTitleBar()
        {
            RequestedTheme = ElementTheme.Dark;

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            Color color = Colors.White;
            Color bgColor = Color.FromArgb(255, 50, 50, 50);
            if (RequestedTheme == ElementTheme.Light)
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
    }
}
