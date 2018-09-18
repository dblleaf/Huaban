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
    public class ListViewModel<T> : ViewModelBase where T : new()
    {
        private IHbService<T> Service { set; get; }
        public ListViewModel(IHbService<T> service)
        {
            Service = service;
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

        private async Task<T> GetData(uint startIndex, int page)
        {
            if (IsLoading)
            {
                return new T();
            }
            IsLoading = true;
            try
            {
                var list = await Service.GetAsync(20, GetMaxId());// CategoryService.GetCategoryPinList(CurrentCategory.nav_link, 20, PinListViewModel.GetMaxPinID());

                if (list.Count() == 0)
                    Data.NoMore();
                else
                    Data.HasMore();
                return list.Pins;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                IsLoading = false;
            }
            return new T();
        }

        private long GetMaxId()
        {
            if (Data?.Count > 0)
            {
                return Data[Data.Count - 1].PinId;
            }
            else
            {
                return 0;
            }

        }
    }
}
