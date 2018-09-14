using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dblleaf.UWP.Huaban.Services.Navigation
{
    public interface IPageWithViewModel<T>
        where T : class
    {
        T ViewModel { get; set; }

        void UpdateBindings();
    }
}