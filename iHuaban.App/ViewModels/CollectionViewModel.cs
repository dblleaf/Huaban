using iHuaban.App.Models;
using iHuaban.Core.Models;

namespace iHuaban.App.ViewModels
{
    public class CollectionModel<T> : ObservableObject where T : IModel
    {
        public Context Context { get; set; }
        public IncrementalLoadingList<T> _Data;
        public IncrementalLoadingList<T> Data
        {
            get { return _Data; }
            set { SetValue(ref _Data, value); }
        }

        private T _SelectedItem;
        public T SelectedItem
        {
            get { return _SelectedItem; }
            set { SetValue(ref _SelectedItem, value); }
        }
    }
}
