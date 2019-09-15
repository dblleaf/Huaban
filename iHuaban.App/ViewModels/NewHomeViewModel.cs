using iHuaban.App.Models;
using iHuaban.App.Services;
using iHuaban.App.TemplateSelectors;
using iHuaban.Core;
using iHuaban.Core.Commands;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace iHuaban.App.ViewModels
{
    public  class NewHomeViewModel : PageViewModel
    {
        private IHomeService HomeService { get; set; }

        public IValueConverter ValueConverter { get; set; }
        public NewHomeViewModel(IHomeService homeService, IValueConverter valueConverter)
        {
            this.HomeService = homeService;
            this.ValueConverter = valueConverter;
            this.Pins = new IncrementalLoadingList<IModel>(GetData);
        }
        public override string TemplateName => Constants.TemplateHome;
        public string ScaleSize => "300:300";
        public decimal CellMinWidth => 236;
        public DataTemplateSelector DataTemplateSelector => new SupperDataTemplateSelector();

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

        private DelegateCommand _GotoTopCommand;
        public DelegateCommand GotoTopCommand
        {
            get
            {
                return _GotoTopCommand ?? (_GotoTopCommand = new DelegateCommand(
                async o =>
                {
                    try
                    {
                        if (o is GridView gridView)
                        {
                            if (gridView?.Items?.Count > 0)
                            {
                                await gridView.ScrollToItem(gridView.Items[0]);
                            }
                        }
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
