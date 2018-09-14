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
    public class PinListViewModel : ViewModelBase
    {
        private string Board { set; get; }
        private IPinsResultService PinsResultService { set; get; }
        private IncrementalLoadingList<Pin> _PinList;
        public IncrementalLoadingList<Pin> PinList
        {
            get { return _PinList; }
            set { SetValue(ref _PinList, value); }
        }
        private bool _IsLoading;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set { SetValue(ref _IsLoading, value); }
        }
        public PinListViewModel(string board, IPinsResultService pinsResultService)
        {
            Board = board;
            PinsResultService = pinsResultService;
            PinList = new IncrementalLoadingList<Pin>(GetData);
        }

        private async Task<IEnumerable<Pin>> GetData(uint startIndex, int page)
        {
            if (Board == null)
                return new List<Pin>();

            IsLoading = true;
            try
            {
                var list = await PinsResultService.GetPinsAsync(20, GetMaxId());// CategoryService.GetCategoryPinList(CurrentCategory.nav_link, 20, PinListViewModel.GetMaxPinID());

                if (list.Count() == 0)
                    PinList.NoMore();
                else
                    PinList.HasMore();
                return list;
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
            return PinList[PinList.Count - 1].PinId;
        }
    }
}
