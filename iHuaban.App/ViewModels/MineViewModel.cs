using iHuaban.App.Models;
using iHuaban.App.TemplateSelectors;
using iHuaban.Core.Helpers;
using iHuaban.Core.Models;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.ViewModels
{
    public class MineViewModel : ViewModelBase
    {
        private IHttpHelper httpHelper;
        private ViewModelBase viewModel;
        public ViewModelBase ViewModel
        {
            get { return viewModel; }
            set { SetValue(ref viewModel, value); }
        }
        public ViewModelBase LoginViewModel { get; private set; }

        public Context Context { get; private set; }

        public MineViewModel(LoginViewModel loginViewModel, IHttpHelper httpHelper, Context context)
        {
            this.LoginViewModel = loginViewModel;
            this.httpHelper = httpHelper;
            this.Context = context;
            this.ViewModel = context.User == null ? LoginViewModel : GetCurrentUserViewModel();
            this.Context.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(this.Context.User))
                {
                    ViewModel = context.User == null ? LoginViewModel : GetCurrentUserViewModel();
                }
            };
        }

        public UserViewModel GetCurrentUserViewModel()
        {
            return new UserViewModel(this.Context.User, this.httpHelper);
        }

        public override string Icon => Constants.IconMine;
        public override string Title => Constants.TextMine;
        public override string TemplateName => Constants.TemplateMine;

        public DataTemplateSelector DataTemplateSelector => new SupperDataTemplateSelector();
    }
}
