using iHuaban.App.Helpers;
using iHuaban.App.Models;
using iHuaban.App.Services;
using iHuaban.Core.Commands;
using iHuaban.Core.Models;
using System;
using System.Collections;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace iHuaban.App.ViewModels
{
    public class ImageViewerViewModel : ViewModelBase
    {
        private IApiHttpHelper httpHelper;
        public Context Context { get; set; }
        private IThemeService themeService;
        private IList List;
        private ListViewBase listView;
        public ImageViewerViewModel(IApiHttpHelper httpHelper,
            IThemeService themeService,
            IAccountService accountService,
            Context context)
        {
            this.httpHelper = httpHelper;
            this.themeService = themeService;
            this.Context = context;
        }

        private Pin _Pin;
        public Pin Pin
        {
            get { return _Pin; }
            private set { SetValue(ref _Pin, value); }
        }

        private Visibility _PreviousVisibility;
        public Visibility PreviousVisibility
        {
            get { return _PreviousVisibility; }
            private set { SetValue(ref _PreviousVisibility, value); }
        }

        private Visibility _NextVisibility;
        public Visibility NextVisibility
        {
            get { return _NextVisibility; }
            private set { SetValue(ref _NextVisibility, value); }
        }

        public ElementTheme GetRequestTheme()
        {
            return this.themeService.RequestTheme;
        }

        public async Task SetPinAsync(ListViewBase listView)
        {
            if (listView.ItemsSource is IList list)
            {
                this.List = list;
                this.listView = listView;
                await this.OnSelect(listView.SelectedIndex);
            }
        }

        public async Task OnSelect(int index)
        {
            this.PreviousVisibility = index > 0 ? Visibility.Visible : Visibility.Collapsed;
            this.NextVisibility = index < this.List.Count - 1 ? Visibility.Visible : Visibility.Collapsed;

            if (index >= 0 && index < this.List.Count)
            {
                this.listView.SelectedIndex = index;
                this.listView.ScrollIntoView(this.listView.Items[index]);
                var pin = this.List[index] as Pin;
                this.Pin = pin;

                if (pin != null)
                {
                    var url = $"pins/{pin.pin_id}";
                    var Dispatcher = Window.Current.Dispatcher;

                    await Task.Run(async () =>
                    {
                        var result = await this.httpHelper.GetAsync<PinResult>(url);
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            this.Pin = result.Pin;
                        });
                    });
                }

            }
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

        private DelegateCommand _SelectCommand;
        public DelegateCommand SelectCommand
        {
            get
            {
                return _SelectCommand ?? (_SelectCommand = new DelegateCommand(
                async o =>
                {
                    try
                    {
                        var index = this.listView.SelectedIndex;
                        if (o.ToString() == "+" && this.listView.SelectedIndex < this.List.Count)
                        {
                            index++;
                        }
                        if (o.ToString() == "-" && this.listView.SelectedIndex > 0)
                        {
                            index--;
                        }
                        await this.OnSelect(index);
                    }
                    catch (Exception)
                    { }

                }, o => true));
            }
        }
    }
}
