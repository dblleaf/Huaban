using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public interface INavigationService
    {
        void Navigate(string pageName, object parameter = null);
        void Navigate(Type pageType, object parameter = null);
        void Navigate<T>(object parameter = null);
    }
}
