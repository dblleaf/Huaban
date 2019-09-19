using iHuaban.App.Helpers;
using iHuaban.App.Models;
using iHuaban.Core.Models;

namespace iHuaban.App.ViewModels
{
    public class MineViewModel : PageViewModel
    {
        private IApiHttpHelper httpHelper;
        private ViewModelBase viewModel;
        public ViewModelBase ViewModel
        {
            get { return viewModel; }
            set { SetValue(ref viewModel, value); }
        }
        public ViewModelBase LoginViewModel { get; private set; }

        public Context Context { get; private set; }

        public MineViewModel(LoginViewModel loginViewModel, IApiHttpHelper httpHelper, Context context)
        {
            this.LoginViewModel = loginViewModel;
            this.httpHelper = httpHelper;
            this.Context = context;
            this.ViewModel = context.User == null ? LoginViewModel : GetCurrentUserViewModel();
            this.Context.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "User")
                {
                    ViewModel = context.User == null ? LoginViewModel : GetCurrentUserViewModel();
                }
            };
        }

        public UserViewModel GetCurrentUserViewModel()
        {
            return new UserViewModel(this.Context.User, this.httpHelper);
        }
    }
}
