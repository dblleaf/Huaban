using iHuaban.App.Models;
using System;
using Windows.UI.Popups;

namespace iHuaban.App.Commands
{
    public class DownloadCommand : Command
    {
        public DownloadCommand(Context context)
            : base(context)
        { }

        public override async void Execute(object parameter)
        {
            await new MessageDialog("Download", "Download").ShowAsync();
        }
    }
}
