using iHuaban.App.Models;
using System;
using Windows.UI.Popups;

namespace iHuaban.App.Commands
{
    public class ToPinCommand : Command
    {
        public ToPinCommand(Context context) 
            : base(context) { }

        public override async void Execute(object parameter)
        {
            await new MessageDialog("ToPinCommand", "ToPinCommand").ShowAsync();
        }
    }
}
