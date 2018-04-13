using Windows.UI.Xaml.Controls;

namespace Dblleaf.UWP.Huaban.Controls
{
    using Dblleaf.UWP.Huaban.Services;
    using Dblleaf.UWP.Huaban.ViewModels;
    using Windows.Foundation;

    public class HBControl<T> : UserControl where T : HBViewModel
    {
        internal string TargetName { set; get; }
        public HBControl()
        {
            ViewModel = ServiceLocator.Resolve<T>();
            this.Loaded += (s, e) =>
            {
                if (!ViewModel.IsInited)
                {
                    ViewModel.Inited();
                }
            };
        }
        public HBFrame Frame { get; internal set; }

        public T ViewModel
        {
            private set
            {
                this.DataContext = value;
            }
            get
            {
                return this.DataContext as T;
            }
        }

        public virtual void OnNavigatingFrom(HBNavigatingCancelEventArgs e)
        {
            ViewModel.OnNavigatingFrom(e);
        }

        public virtual void OnNavigatedFrom(HBNavigationEventArgs e)
        {
            ViewModel.OnNavigatedFrom(e);
        }

        public virtual void OnNavigatedTo(HBNavigationEventArgs e)
        {
            ViewModel.OnNavigatedTo(e);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            ViewModel?.ArrangeOverride(finalSize);
            return base.ArrangeOverride(finalSize);
        }
    }
}
