using iHuaban.App.Helpers;
using iHuaban.App.Models;
using iHuaban.App.Services;
using iHuaban.Core.Commands;
using iHuaban.Core.Models;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace iHuaban.App.ViewModels
{
    public class ImageViewerViewModel : ViewModelBase
    {
        private IApiHttpHelper httpHelper;
        public Context Context { get; set; }
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
            private set { SetValue(ref _Pin, value); }
        }

        public ElementTheme GetRequestTheme()
        {
            return this.themeService.RequestTheme;
        }

        public async Task SetPinAsync(Pin pin)
        {
            var url = $"pins/{pin.pin_id}";
            var Dispatcher = Window.Current.Dispatcher;
            this.Pin = pin;
            await Task.Run(async () =>
            {
                var result = await this.httpHelper.GetAsync<PinResult>(url);
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    this.Pin = result.Pin;
                });
            });
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
