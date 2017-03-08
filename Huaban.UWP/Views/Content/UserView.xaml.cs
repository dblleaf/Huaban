using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Huaban.UWP.Views
{
    using Controls;
    using Models;
    using ViewModels;
    public sealed partial class UserView : HBControl
    {
        public UserView()
        {
            this.InitializeComponent();
        }

        private void pivot_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            var ele = pivot.ContainerFromIndex(pivot.SelectedIndex) as FrameworkElement;

            ele.Loaded += (s, args) =>
            {
                var sv = ele.GetChild<ScrollViewer>();
                if (sv == null)
                    return;

                sv.ViewChanged += OnViewChanged;
            };
        }

        private void OnViewChanged(object sender, ScrollViewerViewChangedEventArgs args)
        {

            ScrollViewer _sv = sender as ScrollViewer;
            if (_sv == null)
                return;
            double y = 0;
            double h = headerInner.ActualHeight;
            if (_sv.VerticalOffset >= h)
            {
                y = h;
            }
            else
            {
                y = _sv.VerticalOffset;
            }


            header.Height = h - y;
        }
    }
}
