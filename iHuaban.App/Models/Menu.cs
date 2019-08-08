using iHuaban.Core.Models;

namespace iHuaban.App.Models
{
    public class Menu
    {
        public string Title { set; get; }
        public string Icon { set; get; }
        public string TemplateName { set; get; }
        public PageViewModel ViewModel { set; get; }
    }
}
