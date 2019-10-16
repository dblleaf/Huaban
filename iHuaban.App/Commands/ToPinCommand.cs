using iHuaban.App.Models;
using iHuaban.App.Views.Content;

namespace iHuaban.App.Commands
{
    public class ToPinCommand : Command
    {
        public ToPinCommand(Context context)
            : base(context) { }

        public override void Execute(object parameter)
        {
            if (parameter is Pin pin)
            {
                ImageViewer.Show(pin);
            }
        }
    }
}
