using iHuaban.App.Services;
using iHuaban.App.ViewModels;
using iHuaban.Core;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.Views
{

    public sealed partial class ShellPage : Page, IPage<ShellViewModel>
    {
        public ShellPage()
        {
            this.InitializeComponent();
            Window.Current.SetTitleBar(coreTitle);
            NavigationService.SetSplitViewFrame(this.rootFrame);
        }

        private void RootSplitView_PaneOpening(SplitView sender, object args)
        {
            try
            {
                SmaillToBigAvatar.Begin();
            }
            catch (Exception) { }

        }

        private void RootSplitView_PaneClosing(SplitView sender, SplitViewPaneClosingEventArgs args)
        {
            try
            {
                BigToSmaillAvatar.Begin();
            }
            catch (Exception) { }
        }
    }
}
