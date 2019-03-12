using iHuaban.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using iHuaban.Core;
using iHuaban.Core.Models;

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
                return (DataTemplate)Application.Current.Resources["Template" + recommend.typeName];
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}
