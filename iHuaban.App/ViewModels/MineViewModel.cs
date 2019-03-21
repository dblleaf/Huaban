using iHuaban.App.Models;
using iHuaban.App.TemplateSelectors;
using iHuaban.Core.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.ViewModels
{
    public class MineViewModel : ViewModelBase
    {
        private ViewModelBase viewModel;
        public ViewModelBase ViewModel
        {
            get { return viewModel; }
            set { SetValue(ref viewModel, value); }
        }
        public ViewModelBase LoginViewModel { get; private set; }
        public ViewModelBase CurrentUserViewModel { get; private set; }
        public Context Context { get; private set; }

        public MineViewModel(LoginViewModel loginViewModel, CurrentUserViewModel currentUserViewModel, Context context)
        {
            this.LoginViewModel = loginViewModel;
            this.CurrentUserViewModel = currentUserViewModel;
            this.Context = context;
            this.ViewModel = context.User == null ? LoginViewModel : CurrentUserViewModel;
            this.Context.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(this.Context.User))
                {
                    ViewModel = context.User == null ? LoginViewModel : CurrentUserViewModel;
                }
            };
        }

        public override string Icon => Constants.IconMine;
        public override string Title => Constants.TextMine;
        public override string TemplateName => Constants.TemplateMine;

        public DataTemplateSelector DataTemplateSelector => new SupperDataTemplateSelector();
    }
}
