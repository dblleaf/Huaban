using iHuaban.App.Models;
using iHuaban.Core.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.TemplateSelectors
{
    public class SupperDataTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var menu = item as ViewModelBase;
            if (menu != null)
            {
                return (DataTemplate)Application.Current.Resources[menu.TemplateName];
            }

            var recommend = item as IModel;
            if (recommend != null)
            {
                return (DataTemplate)Application.Current.Resources["Template" + recommend.typeName + "Item"];
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}
