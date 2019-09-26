using iHuaban.App.Models;
using iHuaban.App.Services;

namespace iHuaban.App.Commands
{
    public class QuickPinCommand : Command
    {
        private IAccountService accountService;
        public QuickPinCommand(Context context, IAccountService accountService)
            : base(context)
        {
            this.accountService = accountService;
        }

        public override async void Execute(object parameter)
        {
            if (parameter is Pin pin && !string.IsNullOrWhiteSpace(this.Context.QuickBoard.board_id))
            {
                var result = await accountService.PickPinAsync(pin, this.Context.QuickBoard.board_id);
                if (result.Pin.pin_id > 0)
                {
                    Context.ShowMessage($"已采集到：{this.Context.QuickBoard.title}");
                }
            }
        }
    }
}

