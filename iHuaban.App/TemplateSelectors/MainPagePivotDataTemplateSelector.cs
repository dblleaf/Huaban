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
        public DataTemplate PinListTemplate { set; get; }
        public DataTemplate FindTemplate { set; get; }
        public DataTemplate MineTemplate { set; get; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var menu = item as Menu;
            if (menu?.Template == Constants.TemplatePinList)
            {
                return PinListTemplate;
            }
            else if (menu?.Template == Constants.TemplateFind)
            {
                return FindTemplate;
            }
            else if (menu?.Template == Constants.TemplateMine)
            {
                return MineTemplate;
            }
            return base.SelectTemplateCore(item, container);
        }
    }
}
