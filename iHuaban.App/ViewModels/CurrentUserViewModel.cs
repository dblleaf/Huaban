using iHuaban.App.Models;
using iHuaban.Core.Models;

namespace iHuaban.App.ViewModels
{
    public class CurrentUserViewModel : ViewModelBase
    {
        public Context Context { get; private set; }
        public override string TemplateName => Constants.TemplateCurrentUser;
        public CurrentUserViewModel(Context context)
        {
            this.Context = context;
            this.Context.PropertyChanged += Context_PropertyChanged;
        }

        private void Context_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(User))
            {
                this.NotifyPropertyChanged(nameof(User));
            }
        }

        public User User { get { return this.Context.User; } }

        public IncrementalLoadingList<Pin> Pins { get; set; } new IncrementalLoadingList<Pin>();
        public IncrementalLoadingList<Board> Boards { get; set; } new IncrementalLoadingList<Board>();
        public IncrementalLoadingList<Pin> Likes { get; set; } new IncrementalLoadingList<Pin>();

    }
}
