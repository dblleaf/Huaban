using iHuaban.App.Services;
using iHuaban.Core.Models;
using System;
using Windows.UI.Xaml;

namespace iHuaban.App.Models
{
    public class Menu
    {
        public string Title { set; get; }
        public string Icon { set; get; }
        public string Template { set; get; }
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
        public DataTemplate ItemTemplate => (DataTemplate)Application.Current.Resources[ItemTemplateName];
    }
}
