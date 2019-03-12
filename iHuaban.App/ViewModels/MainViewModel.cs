using iHuaban.App.Models;
using iHuaban.App.Services;
using iHuaban.App.TemplateSelectors;
using iHuaban.Core.Commands;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
            this.navigationService = navigationService;
            this.DataTemplateSelector = dataTemplateSelector;
        }

        public override async Task InitAsync()
        {
            if (IsInit)
                return;
            IsInit = true;

            var list = new List<Menu>()
            {
                new Menu
                {
                    Title = Constants.TextHome,
                    Icon = Constants.IconHome,
                    TemplateName = Constants.TemplateHome,
                    CellMinWidth = 236,
                    ScaleSize = "300:300",
                    ItemTemplateSelector = new SupperDataTemplateSelector(),
                    ViewModelType = typeof(HomeViewModel),
                },
                new Menu
                {
                    Title = Constants.TextCategory,
                    Icon = Constants.IconCategory, //"\uE721",
                    CellMinWidth = 236,
                    TemplateName = Constants.TemplateCategories,
                    ItemTemplateSelector = new SupperDataTemplateSelector(),
                    ViewModelType = typeof(CategoryViewModel),
                },
                new Menu
                {
                    Title = Constants.TextFind,
                    Icon = Constants.IconFind, //"\uE721",
                    CellMinWidth = 236,
                    ScaleSize = "300:300",
                    TemplateName = Constants.TemplateSearch,
                    ItemTemplateSelector = new SupperDataTemplateSelector(),
                    ViewModelType = typeof(SearchViewModel),
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
