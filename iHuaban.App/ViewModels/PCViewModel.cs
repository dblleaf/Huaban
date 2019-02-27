using iHuaban.App.Models;
using iHuaban.App.Services;

namespace iHuaban.App.ViewModels
{
    public class PCViewModel : PinListViewModel
    {
        public PCViewModel(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        protected override string GetApiUrl()
        {
            return Constants.ApiBoards + Constants.ApiPCBoard + "/" + Constants.ApiPinsName;
        }
    }
}
