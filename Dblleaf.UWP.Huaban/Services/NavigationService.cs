using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Dblleaf.UWP.Huaban.Services
{
    using Dblleaf.UWP.Huaban.Controls;
    using Dblleaf.UWP.Huaban.ViewModels;

    public class ButtonVisibilityChangedEventArgs : EventArgs
    {
        public Visibility Visibility { get; set; }
        public ButtonVisibilityChangedEventArgs(Visibility visibility)
        {
            Visibility = visibility;
        }
    }
    public class NavigationService
    {
        public event EventHandler<BackRequestedEventArgs> BackEvent;
        public event EventHandler<ButtonVisibilityChangedEventArgs> ButtonVisibilityChanged;
        private HBFrame HBFrame { get; set; }
        private Frame Frame { get; set; }
        private Context Context { get; set; }
        public NavigationService(Context context)
        {
            Context = context;
        }

        private void NavigationService_BackRequested(object sender, BackRequestedEventArgs e)
        {
            BackEvent?.Invoke(sender, e);
            bool handled = e.Handled;

            if (!e.Handled)
                this.BackRequested(ref handled);

            e.Handled = handled;
        }

        private bool b;
        private void BackRequested(ref bool handled)
        {
            if (this.CanGoBack)
            {
                handled = true;
                this.GoBack();
            }
            else if (!this.CanGoBack && !handled)
            {
                if (b)
                {
                    App.Current.Exit();
                }
                else
                {
                    b = true;
                    handled = true;
                    Task.Run(async () =>
                    {
                        Context.ShowTip("再按一次退出");
                        await Task.Delay(1500);
                        b = false;
                    });
                }
            }
        }

        public void SetFrame(Frame MainFrame, HBFrame detailFrame)
        {
            Frame = MainFrame;
            HBFrame = detailFrame;
            SystemNavigationManager.GetForCurrentView().BackRequested += NavigationService_BackRequested;
        }

        public void MenuNavigateTo(Type sourcePageType, object parameter = null)
        {
            Frame.Navigate(sourcePageType, parameter);
            DisplayBackButton();
        }

        public void NavigateTo(Type sourcePageType, object parameter = null, string targetName = null)
        {
            HBFrame.Navigate(sourcePageType, parameter, targetName);
            DisplayBackButton();
        }

        public void GoBack()
        {
            try
            {
                if (HBFrame.CanGoBack)
                    HBFrame.GoBack();
                else if (Frame.CanGoBack)
                    Frame.GoBack();


                DisplayBackButton();
            }
            catch (Exception ex)
            {
                string a = ex.Message;
            }

        }
        public bool CanGoBack
        {
            get
            {
                return HBFrame.CanGoBack || Frame.CanGoBack;
            }
        }
        public void DisplayBackButton()
        {
            if (CanGoBack)
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            else
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

            ButtonVisibilityChanged?.Invoke(SystemNavigationManager.GetForCurrentView(), new ButtonVisibilityChangedEventArgs(CanGoBack ? Visibility.Visible : Visibility.Collapsed));
        }
    }
}