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
using Windows.UI.Xaml.Navigation;

namespace iHuaban.App.ViewModels
{
    public class MainViewModel : PageViewModel
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
            MineViewModel mineViewModel
        )
        {
            this.navigationService = navigationService;
            this.storageService = storageService;
            var list = new List<ViewModelBase>
            {
                homeViewModel,
                findViewModel,
                mineViewModel
            };
            Menu = new ObservableCollection<ViewModelBase>(list);
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
                    catch (Exception)
                    {

                    }

                }, o => true));
            }
        }
    }
}
