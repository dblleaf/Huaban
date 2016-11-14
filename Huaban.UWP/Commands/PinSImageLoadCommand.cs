using System;
using System.Windows.Input;

namespace Huaban.UWP.Commands
{
    using Models;
    public class PinSImageLoadCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var pin = parameter as Pin;
            if (pin != null)
            {
                pin.IsLoaded = true;
            }
        }
    }
}
