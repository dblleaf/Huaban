using iHuaban.App.Models;
using iHuaban.App.Services;
using iHuaban.Core.Commands;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace iHuaban.App.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private IHomeService HomeService { get; set; }
        public HomeViewModel(IHomeService homeService)
        {
            this.HomeService = homeService;
            this.Pins = new IncrementalLoadingList<IModel>(GetData);
        }

        private IncrementalLoadingList<IModel> _Recommends;
        public IncrementalLoadingList<IModel> Pins
        {
            get { return _Recommends; }
            set { SetValue(ref _Recommends, value); }
        }

        public ObservableCollection<Explore> Explores { get; set; }

        private DelegateCommand _RefreshCommand;
        public DelegateCommand RefreshCommand
        {
            get
            {
                return _RefreshCommand ?? (_RefreshCommand = new DelegateCommand(
                async o =>
                {
                    try
                    {
                        currentPage = 1;
                        await this.Pins.ClearAndReload();
                    }
                    catch (Exception)
                    {

                    }

                }, o => true));
            }
        }

        private int currentPage = 1;
        private async Task<IEnumerable<IModel>> GetData(uint startIndex, int page)
        {
            if (IsLoading || currentPage > 5)
            {
                return new List<IModel>();
            }
            IsLoading = true;
            try
            {
                var home = await HomeService.GetPagingHomeAsync(currentPage++);

                if (home.Recommends.Count > 0)
                    Pins.HasMore();
                else
                    Pins.NoMore();

                if (home.Explores?.Count > 0 && !(Explores?.Count > 0))
                {
                    Explores = new ObservableCollection<Explore>(home.Explores);
                    NoMoreVisibility = Visibility.Visible;
                }

                return home.Recommends;
            }
            catch
            {
            }
            finally
            {
                IsLoading = false;
            }

            return null;
        }

    }
}
