using iHuaban.App.Models;
using System;
using System.Windows.Input;
using Windows.UI.Popups;

namespace iHuaban.App.Commands
{
    public class PinCommand : ICommand
    {
        private Context context;
        public event EventHandler CanExecuteChanged;
        public PinCommand(Context context)
        {
            this.context = context;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.context.PickPin(parameter as Pin);
        }
    }
}

