using iHuaban.Core.Commands;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace iHuaban.Core.Models
{
    public abstract class PageViewModel : ViewModelBase
    {

        public virtual async Task InitAsync()
        {
            await Task.FromResult(0);
        }

        private DelegateCommand _PageLoadedCommand;
        public DelegateCommand PageLoadedCommand
        {
            get
            {
                return _PageLoadedCommand ?? (_PageLoadedCommand = new DelegateCommand(
                async o =>
                {
                    try
                    {
                        await this.InitAsync();
                    }
                    catch (Exception)
                    {

                    }

                }, o => true));
            }
        }

    }
}
