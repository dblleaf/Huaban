using iHuaban.App.Models;
using System;
using Windows.UI.Popups;

namespace iHuaban.App.Commands
{
    public class ToBoardCommand : Command
    {
        public ToBoardCommand(Context context) 
            : base(context) { }

        public override async void Execute(object parameter)
        {
            await new MessageDialog("ToBoardCommand", "ToBoardCommand").ShowAsync();
        }
    }
}

