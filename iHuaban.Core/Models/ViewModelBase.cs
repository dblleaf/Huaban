using System.Threading.Tasks;

namespace iHuaban.Core.Models
{
    public class ViewModelBase : ObservableObject
    {
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
    }
}
