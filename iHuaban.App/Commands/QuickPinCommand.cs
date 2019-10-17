using iHuaban.App.Models;
using iHuaban.App.Services;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;

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
            if (this.Context.User == null || string.IsNullOrWhiteSpace(this.Context.User.user_id))
            {
                this.Context.ShowMessage("没有登录！");
                return;
            }

            if (parameter is Pin pin && !string.IsNullOrWhiteSpace(this.Context.QuickBoard.board_id))
            {
                var dispatcher = Window.Current.Dispatcher;
                await Task.Run(async () =>
                {
                    var result = await accountService.PickPinAsync(pin, this.Context.QuickBoard.board_id);
                    if (result.Pin.pin_id > 0)
                    {
                        await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            Context.ShowMessage($"已采集到：{this.Context.QuickBoard.title}");
                        });
                    }
                });

            }
        }
    }
}

