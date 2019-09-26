using iHuaban.App.Models;

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
                this.Context.PickPin(pin);
            }
        }
    }
}

