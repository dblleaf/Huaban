using iHuaban.App.Models;
using iHuaban.Core.Models;
using System.Collections.ObjectModel;

namespace iHuaban.App.ViewModels
{
    public class AboutViewModel: PageViewModel
    {
        public ObservableCollection<SimpleModel> AboutList { set; get; } = new ObservableCollection<SimpleModel>(new SimpleModel[] {
            new SimpleModel { Title = "应用名称", Label = "爱花瓣UWP" },
            new SimpleModel { Title = "声明", Label = "第三方App，非官方"},
            new SimpleModel { Title = "开源地址", Label = "http://github.com/dblleaf/huaban" },
            new SimpleModel { Title = "作者", Label = "song-zhaoli" },
            new SimpleModel { Title = "Email", Label = "song-zhaoli@outlook.com" },
            new SimpleModel { Title = "微博", Label = "@宋小召召" },
            new SimpleModel { Title = "QQ群", Label = "534867173" }
        });
    }
}
