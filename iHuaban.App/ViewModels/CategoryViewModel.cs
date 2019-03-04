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

        private DelegateCommand _ChangeCategoryCommand;
        public DelegateCommand ChangeCategoryCommand
        {
            get
            {
                return _ChangeCategoryCommand ?? (_ChangeCategoryCommand = new DelegateCommand(
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
                var list = await this.ServiceProvider.GetAsync<PinCollection>(GetCategoryPinsUrl(), 20, GetMaxPinId());// CategoryService.GetCategoryPinList(CurrentCategory.nav_link, 20, PinListViewModel.GetMaxPinID());

                if (list.Data.Count() == 0)
                    PinsData.NoMore();
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
        protected override async Task<IEnumerable<Category>> GetData(uint startIndex, int page)
        {
            var list = await base.GetData(startIndex, page);
            Data.Add(new Category() { name = "最热", nav_link = "/popular/" });
            Data.Add(new Category() { name = "最新", nav_link = "/all/" });
            return list;
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
            string categoryLink = Constants.DefaultFavorite;
            if (SelectedItem != null)
            {
                categoryLink = SelectedItem.nav_link;
            }
            return Constants.ApiBase + categoryLink.TrimStart('/');
        }

        protected override string GetApiUrl()
        {
            return Constants.ApiCategories;
        }
    }
}