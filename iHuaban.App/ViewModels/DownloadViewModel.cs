using iHuaban.App.Models;
using iHuaban.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace iHuaban.App.ViewModels
{
    public class DownloadViewModel : PageViewModel
    {
        public ObservableCollection<PinDownloadItem> Data { get; set; } = new ObservableCollection<PinDownloadItem>();
        public Context Context { get; set; }

        public void AddDownloadQueue(Pin pin)
        {
            Data.Add(new PinDownloadItem { Pin = pin });
            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.DoWork += Worker_DoWork;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private async void Run()
        {
            var Dispatcher = Window.Current.Dispatcher;

            await Task.Run(async () =>
            {
                HttpClient client = new HttpClient();
                while (true)
                {
                    var pin = Data.FirstOrDefault();
                    if (pin != null)
                    {
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {

                        });
                    }
                    await Task.Delay(500);
                }
            });
        }
    }
}
