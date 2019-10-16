using iHuaban.App.Models;
using iHuaban.App.Services;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.Web.Http;

namespace iHuaban.App.Commands
{
    public class DownloadCommand : Command
    {
        private IStorageService storageService;
        public DownloadCommand(Context context, IStorageService storageService)
            : base(context)
        {
            this.storageService = storageService;
        }

        public override async void Execute(object parameter)
        {
            if (parameter is Pin pin)
            {
                string type = pin.file.type?.ToLower();
                if (type?.IndexOf("png") >= 0)
                    type = "png";
                else if (type?.IndexOf("bmp") >= 0)
                    type = "bmp";
                else if (type?.IndexOf("gif") >= 0)
                    type = "gif";
                else
                    type = "jpg";
                try
                {
                    var Dispatcher = Window.Current.Dispatcher;
                    await Task.Run(async () =>
                    {
                        HttpClient httpClient = new HttpClient();
                        var buffer = await httpClient.GetBufferAsync(new Uri(pin.file.Orignal));
                        var path = storageService.GetSetting(Constants.SavePath);
                        StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(path);
                        var file = await storageFolder.CreateFileAsync($"{pin.file.key}.{type}");
                        await FileIO.WriteBufferAsync(file, buffer);

                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            this.Context.ShowMessage("下载成功！");
                        });
                    });
                }
                catch (Exception ex)
                {
                    this.Context.ShowMessage($"下载失败：{ex.Message}");
                }
            }
        }
    }
}
