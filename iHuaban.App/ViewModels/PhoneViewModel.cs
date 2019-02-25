using iHuaban.App.Services;

namespace iHuaban.App.ViewModels
{
    public class PhoneViewModel : PinListViewModel
    {
        public PhoneViewModel(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        protected override string GetApiUrl()
        {
            return Constants.ApiBoards + Constants.ApiPhoneBoard + "/" + Constants.ApiPinsName;
        }
    }
}
