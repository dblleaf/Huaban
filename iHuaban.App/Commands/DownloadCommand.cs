using iHuaban.App.Models;
using System;
using System.Windows.Input;
using Windows.UI.Popups;

namespace iHuaban.App.Commands
{
    public class DownloadCommand : ICommand
    {
        private Context context;
        public event EventHandler CanExecuteChanged;
        public DownloadCommand(Context context)
        {
            this.context = context;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            await new MessageDialog("Download", "Download").ShowAsync();
        }
    }
}
