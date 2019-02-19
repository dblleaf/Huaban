using iHuaban.App.Models;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHuaban.Core;
using iHuaban.App.Services;
using iHuaban.Core.Helpers;

namespace iHuaban.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private HttpHelper httpHelper => new HttpHelper();
        public ObservableCollection<Menu> Menu => new ObservableCollection<Menu>
        {
            new Menu
            {
                Title = Constants.TextPhone,
                Icon = "\uE8EA",
                Template = Constants.TemplateGrid,
                CellMinWidth = 236,
                ScaleSize = "750:1334",
                ItemTemplateName = Constants.TemplatePhone,
                ViewModel = new PinListViewModel<Board>(new BoardService(Constants.ApiPhoneBoard, httpHelper))
            },
            new Menu
            {
                Title = Constants.TextPC,
                Icon = "\uE977",
                Template = Constants.TemplateGrid,
                CellMinWidth = 360,
                ScaleSize = "1920:1080",
                ItemTemplateName = Constants.TemplatePC,
                ViewModel = new PinListViewModel<Board>(new BoardService(Constants.ApiPCBoard, httpHelper))
            },
            new Menu
            {
                Title = Constants.TextFind,
                Icon = "\uE721",
                CellMinWidth = 236,
                Template = Constants.TemplateFind,
                ItemTemplateName = Constants.TemplateCategory,
                ViewModel = new FindViewModel(new HbService<CategoryCollection>(Constants.ApiCategoriesName, httpHelper)),
            },
            new Menu
            {
                Title = Constants.TextMine,
                Icon = "\uE77B",
                Template = Constants.TemplateMine,
                ViewModel = new MineViewModel()
            }
        };
    }
}
