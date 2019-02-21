using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.Views
{
    public partial class PageWithModel<T> : Page
        where T : new()
    {
        private T _ViewModel;
        public T ViewModel
        {
            get
            {
                if (_ViewModel == null)
                {
                    _ViewModel = new T();
                }

                return _ViewModel;
            }
        }
    }
}
