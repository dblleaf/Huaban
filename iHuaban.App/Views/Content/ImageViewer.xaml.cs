using iHuaban.App.Models;
using iHuaban.App.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace iHuaban.App.Views.Content
{
    public sealed partial class ImageViewer : UserControl
    {
        private Popup popup;
        public ImageViewer()
        {
            this.InitializeComponent();
            this.SetBounds();
            Window.Current.SizeChanged += Current_SizeChanged;

            popup = new Popup();
            popup.Child = this;
            this.ViewModel = UnityConfig.ResolveObject<ImageViewerViewModel>();
            this.ViewModel.Parent = this.popup;
            this.DataContext = this.ViewModel;
        }

        private ImageViewerViewModel ViewModel { get; set; }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            SetBounds();
        }

        private void SetBounds()
        {
            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;
        }
        private async void OnShow(Pin pin)
        {
            popup.RequestedTheme = this.ViewModel.GetRequestTheme();
            popup.IsOpen = true;
            await this.ViewModel.SetPinAsync(pin);
        }

        private static ImageViewer instance;
        internal static void Show(Pin pin)
        {
            instance = instance ?? new ImageViewer();
            instance.OnShow(pin);
        }
    }
}
