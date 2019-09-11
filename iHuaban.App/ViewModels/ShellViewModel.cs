using iHuaban.App.Services;
using iHuaban.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.ViewModels
{
    public class ShellViewModel : PageViewModel
    {
        private INavigationService navigationService;
        private IStorageService storageService;
        public ObservableCollection<ViewModelBase> Menu { get; set; }
        public DataTemplateSelector DataTemplateSelector { get; private set; }
        public ShellViewModel(
            INavigationService navigationService,
            IStorageService storageService,
            HomeViewModel homeViewModel,
            FindViewModel findViewModel,
            MineViewModel mineViewModel,
            SettingViewModel settingViewModel,
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
                mineViewModel,
                settingViewModel
            };
            Menu = new ObservableCollection<ViewModelBase>(list);
        }
    }
}
