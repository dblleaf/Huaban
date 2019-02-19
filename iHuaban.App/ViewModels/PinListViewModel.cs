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
    public class PinListViewModel<T> : ListViewModel<T>
        where T : IModel, new()
    {
        private IPinsResultService<ModelCollection<T>> PinsResultService { set; get; }
        private IncrementalLoadingList<Pin> _PinsData;
        public IncrementalLoadingList<Pin> PinsData
        {
            get { return _PinsData; }
            set { SetValue(ref _PinsData, value); }
        }

        public PinListViewModel(IPinsResultService<ModelCollection<T>> pinsResultService)
            : base(pinsResultService)
        {
            PinsResultService = pinsResultService;
            PinsData = new IncrementalLoadingList<Pin>(GetPinsData);
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
                var list = await PinsResultService.GetPinsAsync(20, GetMaxPinId());// CategoryService.GetCategoryPinList(CurrentCategory.nav_link, 20, PinListViewModel.GetMaxPinID());

                if (list.Data.Count() == 0)
                    PinsData.NoMore();
                else
                    PinsData.HasMore();
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
    }
}
