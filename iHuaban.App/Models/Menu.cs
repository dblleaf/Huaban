using iHuaban.Core.Models;
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
        public ViewModelBase ViewModel { set; get; }
        public DataTemplate ItemTemplate=> (DataTemplate)Application.Current.Resources[ItemTemplateName];
    }
}
