using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;

namespace Dblleaf.UWP.Huaban.Helpers
{
    public class TitleBarHelper : ObservableObject
    {
        private static CoreApplicationViewTitleBar _coreTitleBar;
        private static TitleBarHelper _instance = new TitleBarHelper();
        public TitleBarHelper()
        {
            _coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
        }
        public static TitleBarHelper Instance
        {
            get
            {
                return _instance;
            }
        }

        public CoreApplicationViewTitleBar TitleBar
        {
            get
            {
                return _coreTitleBar;
            }
        }
    }
}
