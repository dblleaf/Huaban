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
            if (this.Context.User == null || string.IsNullOrWhiteSpace(this.Context.User.user_id))
            {
                this.Context.ShowMessage("没有登录！");
                return;
            }

            if (parameter is Pin pin)
            {
                PickPinPane.Show(pin);
            }
        }
    }
}

