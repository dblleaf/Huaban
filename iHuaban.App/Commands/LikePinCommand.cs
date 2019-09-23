using System;
using System.Windows.Input;
using Windows.UI.Popups;

namespace iHuaban.App.Commands
{
    public class LikePinCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            await new MessageDialog("LikePinCommand", "LikePinCommand").ShowAsync();
        }
    }
}

