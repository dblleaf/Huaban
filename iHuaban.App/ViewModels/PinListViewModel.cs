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
    public class PinListViewModel : ListViewModel<Pin> 
    {
        private IPinsResultService<Pin> PinsResultService { set; get; }

        public PinListViewModel(IPinsResultService<Pin> pinsResultService)
            : base(pinsResultService)
        {
            PinsResultService = pinsResultService;
            Data = new IncrementalLoadingList<Pin>(GetData);
        }

        private async Task<IEnumerable<Pin>> GetData(uint startIndex, int page)
        {
            if (IsLoading)
            {
                return new List<Pin>();
            }
            IsLoading = true;
            try
            {
                var list = await PinsResultService.GetPinsAsync(20, GetMaxId());// CategoryService.GetCategoryPinList(CurrentCategory.nav_link, 20, PinListViewModel.GetMaxPinID());

                if (list.Pins.Count() == 0)
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
            return null;
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
