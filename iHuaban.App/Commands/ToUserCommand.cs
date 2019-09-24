using iHuaban.App.Models;
using System;
using Windows.UI.Popups;

namespace iHuaban.App.Commands
{
    public class ToUserCommand : Command
    {
        public ToUserCommand(Context context)
            : base(context) { }

        public override async void Execute(object parameter)
        {
            await new MessageDialog("ToUserCommand", "ToUserCommand").ShowAsync();
        }
    }
}

