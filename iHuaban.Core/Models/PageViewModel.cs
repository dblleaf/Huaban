using iHuaban.Core.Commands;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace iHuaban.Core.Models
{
    public abstract class PageViewModel : ViewModelBase
    {

        public virtual void Init() { }

        private DelegateCommand _PageLoadedCommand;
        public DelegateCommand PageLoadedCommand
        {
            get
            {
                return _PageLoadedCommand ?? (_PageLoadedCommand = new DelegateCommand(
                o =>
                {
                    try
                    {
                        this.Init();
                    }
                    catch (Exception)
                    { }
                }, o => true));
            }
        }
    }
}
