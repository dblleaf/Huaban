using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace iHuaban.Core.Models
{
    public class ViewModelBase : ObservableObject
    {
        public ViewModelBase()
        {
            this.NoMoreVisibility = Visibility.Collapsed;
        }

        public Setting Setting { get; private set; } = Setting.Instance();

        public virtual async Task InitAsync()
        {
            await Task.FromResult(0);
        }

        private bool _IsLoading;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set { SetValue(ref _IsLoading, value); }
        }

        private Visibility _NoMoreVisibility;
        public Visibility NoMoreVisibility
        {
            get { return _NoMoreVisibility; }
            set { SetValue(ref _NoMoreVisibility, value); }
        }
    }
}
