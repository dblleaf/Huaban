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
    public class ListViewModel<T> : ViewModelBase
        where T : IModel, new()
    {
        private IService<ModelCollection<T>> Service { set; get; }
        private bool ISupportIncrementalLoading { set; get; }
        public ListViewModel(IService<ModelCollection<T>> service, bool isSpportIncrementalLoading = true)
        {
            Service = service;
            ISupportIncrementalLoading = isSpportIncrementalLoading;
            Data = new IncrementalLoadingList<T>(GetData);
        }
        private IncrementalLoadingList<T> _Data;
        public IncrementalLoadingList<T> Data
        {
            get { return _Data; }
            set { SetValue(ref _Data, value); }
        }
        private bool _IsLoading;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set { SetValue(ref _IsLoading, value); }
        }

        private string _Url;
        public string Url
        {
            get { return _Url; }
            set { SetValue(ref _Url, value); }
        }

        public int Count => throw new NotImplementedException();


        private async Task<IEnumerable<T>> GetData(uint startIndex, int page)
        {
            if (IsLoading)
            {
                return null;
            }
            IsLoading = true;
            try
            {
                var list = await Service.GetAsync(this.Url, 20, GetMaxId());// CategoryService.GetCategoryPinList(CurrentCategory.nav_link, 20, PinListViewModel.GetMaxPinID());

                if (ISupportIncrementalLoading && list?.Count > 0)
                    Data.HasMore();
                else
                    Data.NoMore();
                return list.Data;
            }
            catch (Exception ex)
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
