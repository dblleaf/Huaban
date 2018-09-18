using iHuaban.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using iHuaban.Core;

namespace iHuaban.App.TemplateSelectors
{
    public class MainPagePivotDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TemplateGrid { set; get; }

        public DataTemplate TemplateFind { set; get; }
        public DataTemplate TemplateMine { set; get; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var menu = item as Menu;
            if (menu?.Template == Constants.TemplateGrid)
            {
                return TemplateGrid;
            }
            else if (menu?.Template == Constants.TemplateFind)
            {
                return TemplateFind;
            }
            else if (menu?.Template == Constants.TemplateMine)
            {
                return TemplateMine;
            }
            return base.SelectTemplateCore(item, container);
        }
    }
}
