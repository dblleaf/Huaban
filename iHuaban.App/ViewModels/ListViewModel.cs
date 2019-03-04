using iHuaban.App.Models;
using iHuaban.App.Services;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHuaban.App.ViewModels
{
    public abstract class ListViewModel<TModel, TCollection> : ViewModelBase
        where TModel : IModel, new()
        where TCollection : IModelCollection<TModel>, new()
    {
        protected abstract string GetApiUrl();
        protected Services.IServiceProvider ServiceProvider { set; get; }
        private bool ISupportIncrementalLoading { set; get; }
        public ListViewModel(Services.IServiceProvider service, bool isSpportIncrementalLoading = true)
        {
            this.ServiceProvider = service;
            this.ISupportIncrementalLoading = isSpportIncrementalLoading;
            this.Data = new IncrementalLoadingList<TModel>(GetData);
        }
        private IncrementalLoadingList<TModel> _Data;
        public IncrementalLoadingList<TModel> Data
        {
            get { return _Data; }
            set { SetValue(ref _Data, value); }
        }

        private TModel _SelectedItem;
        public TModel SelectedItem
        {
            get { return _SelectedItem; }
            set { SetValue(ref _SelectedItem, value); }
        }

        public int Count { get { return this.Data.Count; } }

        protected virtual async Task<IEnumerable<TModel>> GetData(uint startIndex, int page)
        {
            if (IsLoading)
            {
                return null;
            }
            IsLoading = true;
            try
            {
                var list = await this.ServiceProvider.GetAsync<TCollection>(GetApiUrl(), 20, GetMaxId());// CategoryService.GetCategoryPinList(CurrentCategory.nav_link, 20, PinListViewModel.GetMaxPinID());

                if (ISupportIncrementalLoading && list?.Count > 0)
                {
                    this.SelectedItem = list.Data.FirstOrDefault();
                    Data.HasMore();
                }
                else
                    Data.NoMore();
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

        private long GetMaxId()
        {
            if (Data?.Count > 0 && long.TryParse(Data?[Data.Count - 1].KeyId, out long maxId))
            {
                return maxId;
            }
            else
            {
                return 0;
            }

        }
    }
}
