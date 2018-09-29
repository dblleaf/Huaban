using iHuaban.App.Models;
using iHuaban.Core.Commands;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public List<SNSType> SnsTypes => new List<SNSType>
        {
            new SNSType { strName = "微博", strType = "weibo", Icon = "/Assets/sina_weibo_50px.png", Url = "http://huaban.com/oauth/weibo/?auth_callback=ms-appx-web:///Assets/auth.html&client_id=1d912cae47144fa09d88&md=com.huaban.android" },
            new SNSType { strName = "QQ", strType = "qzone", Icon = "/Assets/qq_50px.png", Url = "http://huaban.com/oauth/qzone/?auth_callback=ms-appx-web:///Assets/auth.html&client_id=1d912cae47144fa09d88&md=com.huaban.android" },
            new SNSType { strName = "豆瓣", strType = "douban", Icon = "/Assets/douban_50px.png", Url = "https://huaban.com/oauth/douban/?auth_callback=ms-appx-web:///Assets/auth.html&client_id=1d912cae47144fa09d88&md=com.huaban.android" },
            new SNSType { strName = "人人", strType = "renren", Icon = "/Assets/renren_50px.png", Url = "https://huaban.com/oauth/renren/?auth_callback=ms-appx-web:///Assets/auth.html&client_id=1d912cae47144fa09d88&md=com.huaban.android" }
        };

        private int _PivotIndex;
        public int PivotIndex
        {
            set => SetValue(ref _PivotIndex, value);
            get => _PivotIndex;
        }

        private string _LoginUri;
        public string LoginUri
        {
            set { SetValue(ref _LoginUri, value); }
            get { return _LoginUri; }
        }

        private Visibility _WebViewVisibility;
        public Visibility WebViewVisibility
        {
            set { SetValue(ref _WebViewVisibility, value); }
            get { return _WebViewVisibility; }
        }

        private DelegateCommand _SetPivotIndexCommand;
        public DelegateCommand SetPivotIndexCommand
        {
            get
            {
                return _SetPivotIndexCommand ?? (_SetPivotIndexCommand = new DelegateCommand(
                    o =>
                    {

                        int idx = 0;
                        int.TryParse(o.ToString(), out idx);
                        PivotIndex = idx;
                    },
                    o => true)
                );
            }
        }

        private DelegateCommand _ShowWebViewCommand;
        public DelegateCommand ShowWebViewCommand
        {
            get
            {
                return _ShowWebViewCommand ?? (_ShowWebViewCommand = new DelegateCommand(
                    o =>
                    {
                        var args = o as SelectionChangedEventArgs;
                        var item = o as SNSType;
                        if (args == null && item == null)
                            return;

                        if (args != null && args.AddedItems.Count > 0)
                            item = args.AddedItems[0] as SNSType;

                        if (item == null)
                            return;

                        LoginUri = item.Url;
                        WebViewVisibility = Visibility.Visible;
                    },
                    o => true)
                );
            }
        }
    }
}
