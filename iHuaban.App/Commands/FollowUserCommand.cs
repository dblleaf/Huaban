using iHuaban.App.Models;
using iHuaban.App.Services;

namespace iHuaban.App.Commands
{
    public class FollowUserCommand : Command
    {
        private IAccountService accountService;
        public FollowUserCommand(Context context, IAccountService accountService)
            : base(context)
        {
            this.accountService = accountService;
        }

        public override async void Execute(object parameter)
        {
            if (parameter is User user)
            {
                var str= await accountService.FollowUserAsync(user);
                user.following = (str != "{}");
                Context.ShowMessage(user.following ? "关注成功" : "关注失败");
            }
        }
    }
}
