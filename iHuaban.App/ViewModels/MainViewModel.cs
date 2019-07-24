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
        private IStorageService storageService;
        public ObservableCollection<ViewModelBase> Menu { get; set; }
        public MainViewModel
        (
            INavigationService navigationService,
            IStorageService storageService,
            HomeViewModel homeViewModel,
            FindViewModel findViewModel,
            MineViewModel mineViewModel,
            DataTemplateSelector dataTemplateSelector
        )
        {
            this.navigationService = navigationService;
            this.storageService = storageService;
            this.DataTemplateSelector = dataTemplateSelector;
            var list = new List<ViewModelBase>
            {
                homeViewModel,
                findViewModel,
                mineViewModel
            };
            Menu = new ObservableCollection<ViewModelBase>(list);
        }

        public override async Task InitAsync()
        {
            await Task.Delay(0);
        }

        public DataTemplateSelector DataTemplateSelector { get; private set; }
        public override string Icon => string.Empty;
        public override string Title => string.Empty;
        public override string TemplateName => string.Empty;

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
