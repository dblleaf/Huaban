using iHuaban.App.Models;
using iHuaban.Core.Commands;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace iHuaban.App.ViewModels
{
    public class CategoryViewModel : ListViewModel<Category, CategoryCollection>
    {
        public CategoryViewModel(iHuaban.App.Services.IServiceProvider service)
           : base(service, false)
        {
            this.CategoryVisibility = Visibility.Collapsed;
            this.PinsData = new IncrementalLoadingList<Pin>(GetPinsData);
            this.Data.Add(Constants.CategoryAll);
            this.Data.Add(Constants.CategoryHot);
            this.SelectedItem = Constants.CategoryAll;
        }

        private Visibility _CategoryVisibility;
        public Visibility CategoryVisibility
        {
            get { return _CategoryVisibility; }
            set { SetValue(ref _CategoryVisibility, value); }
        }

        private IncrementalLoadingList<Pin> _PinsData;
        public IncrementalLoadingList<Pin> PinsData
        {
            get { return _PinsData; }
            set { SetValue(ref _PinsData, value); }
        }

        private DelegateCommand _SetCategoryVisibilityCommand;
        public DelegateCommand SetCategoryVisibilityCommand
        {
            get
            {
                return _SetCategoryVisibilityCommand ?? (_SetCategoryVisibilityCommand = new DelegateCommand(
                o =>
                {
                    try
                    {
                        this.CategoryVisibility = o.ToString().Equals("true", StringComparison.CurrentCultureIgnoreCase) ? Visibility.Visible : Visibility.Collapsed;
                    }
                    catch (Exception ex)
                    {

                    }

                }, o => true));
            }
        }

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
                        await this.PinsData.ClearAndReload();
                    }
                    catch (Exception ex)
                    {

                    }

                }, o => true));
            }
        }

        private async Task<IEnumerable<Pin>> GetPinsData(uint startIndex, int page)
        {
            if (IsLoading)
            {
                return new List<Pin>();
            }
            IsLoading = true;
            try
            {
                var list = await this.ServiceProvider.GetAsync<PinCollection>(GetCategoryPinsUrl(), 20, GetMaxPinId());

                if (list.Data.Count() == 0)
                {
                    NoMoreVisibility = Visibility.Visible;
                    PinsData.NoMore();
                }
                else
                    PinsData.HasMore();
                return list.Data;
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

        private long GetMaxPinId()
        {
            if (PinsData?.Count > 0 && long.TryParse(PinsData?[PinsData.Count - 1].KeyId, out long maxId))
            {
                return maxId;
            }
            else
            {
                return 0;
            }
        }

        private string GetCategoryPinsUrl()
        {
            return Constants.ApiBase + SelectedItem.nav_link.TrimStart('/');
        }

        protected override string GetApiUrl()
        {
            return Constants.ApiCategories;
        }
    }
}