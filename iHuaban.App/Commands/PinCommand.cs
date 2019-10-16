using iHuaban.App.Models;
using iHuaban.App.Views;

namespace iHuaban.App.Commands
{
    public class PinCommand : Command
    {
        public PinCommand(Context context)
            : base(context) { }

        public override void Execute(object parameter)
        {
            if (parameter is Pin pin)
            {
                PickPinPane.Show(pin);
            }
        }
    }
}

