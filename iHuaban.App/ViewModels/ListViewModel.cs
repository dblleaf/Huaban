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
    public class ListViewModel<T, T2> : ViewModelBase
        where T : IModelCollection<T2>, new()
        where T2 : IModel, new()
    {
        private IHbService<T> Service { set; get; }
        private bool ISupportIncrementalLoading { set; get; }
        public ListViewModel(IHbService<T> service, bool isSpportIncrementalLoading = true)
        {
            Service = service;
            ISupportIncrementalLoading = isSpportIncrementalLoading;
            Data = new IncrementalLoadingList<T2>(GetData);
        }
        private IncrementalLoadingList<T2> _Data;
        public IncrementalLoadingList<T2> Data
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

        public int Count => throw new NotImplementedException();


        private async Task<IEnumerable<T2>> GetData(uint startIndex, int page)
        {
            if (IsLoading)
            {
                return null;
            }
            IsLoading = true;
            try
            {
                var list = await Service.GetAsync(20, GetMaxId());// CategoryService.GetCategoryPinList(CurrentCategory.nav_link, 20, PinListViewModel.GetMaxPinID());

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
