using iHuaban.App.Models;
using iHuaban.App.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace iHuaban.App.Views
{
    public sealed partial class PickPinPane : UserControl
    {
        private Popup popup;
        public PickPinPane()
        {
            this.InitializeComponent();
            this.SetBounds();
            Window.Current.SizeChanged += Current_SizeChanged;

            popup = new Popup();
            popup.Child = this;
            this.ViewModel = UnityConfig.ResolveObject<PickPinPaneViewModel>();
            this.ViewModel.Parent = this.popup;
            this.DataContext = this.ViewModel;
        }

        private PickPinPaneViewModel ViewModel { get; set; }
        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            SetBounds();
        }

        private void SetBounds()
        {
            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;
        }

        private void OnShow(Pin pin)
        {
            popup.RequestedTheme = this.ViewModel.GetRequestTheme();
            this.ViewModel.Pin = pin;
            this.BoardListView.SelectedIndex = -1;
            popup.IsOpen = true;
        }

        private static PickPinPane instance;
        internal static void Show(Pin pin)
        {
            instance = instance ?? new PickPinPane();
            instance.OnShow(pin);
        }
    }
}
