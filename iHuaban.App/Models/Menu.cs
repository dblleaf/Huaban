using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.Models
{
    public class Menu
    {
        public string Title { set; get; }
        public string Icon { set; get; }
        public string Template { set; get; }
        public string ApiBoard { set; get; }
        public ViewModelBase ViewModel { set; get; }
    }
}
