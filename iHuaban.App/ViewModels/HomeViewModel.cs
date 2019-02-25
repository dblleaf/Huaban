using iHuaban.App.Models;
using iHuaban.App.Services;
using iHuaban.Core.Models;

namespace iHuaban.App.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private IService<PinCollection> PinService { get; set; }
        public HomeViewModel(IService<PinCollection> pinService)
        {
            PinService = pinService;
        }
    }
}
