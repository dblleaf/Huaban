using iHuaban.App.Models;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHuaban.Core;
namespace iHuaban.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<Menu> Menu { set; get; } = new ObservableCollection<Menu>
        {
            new Menu
            {
                Title = Constants.MenuPhone,
                Icon = "\uE8EA",
                Template = Constants.TemplatePinList,
                ApiBoard = Constants.ApiPhoneBoard
            },
            new Menu
            {
                Title = Constants.MenuPC,
                Icon = "\uE770",
                Template = Constants.TemplatePinList,
                ApiBoard = Constants.ApiPCBoard
            },
            new Menu
            {
                Title = Constants.MenuFind,
                Icon = "\uE721",
                Template = Constants.TemplateFind
            },
            new Menu
            {
                Title = Constants.MenuMine,
                Icon = "\uE77B",
                Template = Constants.TemplateMine
            }
        };
    }
}
