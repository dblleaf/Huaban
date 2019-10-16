using iHuaban.App.Helpers;
using iHuaban.App.Models;
using iHuaban.App.Services;
using iHuaban.Core.Commands;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace iHuaban.App.ViewModels
{
    public class ImageViewerViewModel : ViewModelBase
    {
        private IApiHttpHelper httpHelper;
        private Context Context;
        private IThemeService themeService;
        private IAccountService accountService;
        public ImageViewerViewModel(IApiHttpHelper httpHelper,
            IThemeService themeService,
            IAccountService accountService,
            Context context)
        {
            this.httpHelper = httpHelper;
            this.themeService = themeService;
            this.accountService = accountService;
            this.Context = context;
        }

        private Pin _Pin;
        public Pin Pin
        {
            get { return _Pin; }
            set { SetValue(ref _Pin, value); }
        }

        public ElementTheme GetRequestTheme()
        {
            return this.themeService.RequestTheme;
        }

        internal Popup Parent;
        private DelegateCommand _HideCommand;
        public DelegateCommand HideCommand
        {
            get
            {
                return _HideCommand ?? (_HideCommand = new DelegateCommand(
                o =>
                {
                    try
                    {
                        if (Parent != null)
                        {
                            Parent.IsOpen = false;
                        }
                    }
                    catch (Exception)
                    { }

                }, o => true));
            }
        }
    }
}
