using iHuaban.App.Models;
using iHuaban.App.Services;

namespace iHuaban.App.Commands
{
    public class LikePinCommand : Command
    {
        private IAccountService accountService;
        public LikePinCommand(Context context, IAccountService accountService)
            : base(context)
        {
            this.accountService = accountService;
        }

        public override async void Execute(object parameter)
        {
            if (parameter is Pin pin)
            {
                var result = await accountService.LikePin(pin);
                pin.like = true;
                Context.ShowMessage("关注成功");
            }
        }
    }
}

