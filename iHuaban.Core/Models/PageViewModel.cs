using iHuaban.Core.Commands;
using System;
using Windows.UI.Xaml;

namespace iHuaban.Core.Models
{
    public abstract class PageViewModel : ViewModelBase, IDisposable
    {
        public virtual void Init() { }

        ~PageViewModel()
        {
            this.Dispose(false);
        }

        public virtual void ViewModelDispose()
        { }

        public void Dispose(bool disposing)
        {
            lock (this)
            {
                if (disposing)
                {
                    this.ViewModelDispose();
                }

                _PageLoadedCommand = null;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

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
