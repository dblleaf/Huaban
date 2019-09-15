﻿using iHuaban.App.ViewModels;
using iHuaban.Core;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.Views
{
    public sealed partial class AboutPage : Page, IView<AboutViewModel>
    {
        public AboutPage()
        {
            this.InitializeComponent();
        }
    }
}
