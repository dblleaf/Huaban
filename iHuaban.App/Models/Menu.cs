using iHuaban.App.Services;
using iHuaban.Core.Models;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.Models
{
    public class Menu
    {
        public string Title { set; get; }
        public string Icon { set; get; }
        public string TemplateName { set; get; }
        public double CellMinWidth { set; get; }
        public string ScaleSize { set; get; }
        public string ItemTemplateName { set; get; }
        private ViewModelBase _ViewModel;
        public ViewModelBase ViewModel
        {
            get
            {
                return _ViewModel ?? (_ViewModel = Locator.ResolveObject<ViewModelBase>(ViewModelType));
            }
        }
        public Type ViewModelType { get; set; }

        public DataTemplateSelector ItemTemplateSelector { get; set; }
    }
}
