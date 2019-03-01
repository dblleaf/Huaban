using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public interface INavigationService
    {
        void Navigate(string pageName);
        void Navigate(Type pageType);
        void Navigate<T>();
    }
}
