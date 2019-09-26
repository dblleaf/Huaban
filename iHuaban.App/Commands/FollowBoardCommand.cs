using iHuaban.App.Models;
using iHuaban.App.Services;

namespace iHuaban.App.Commands
{
    public class FollowBoardCommand : Command
    {
        private IAccountService accountService;
        public FollowBoardCommand(Context context, IAccountService accountService)
            : base(context)
        {
            this.accountService = accountService;
        }

        public override async void Execute(object parameter)
        {
            if (parameter is Board board)
            {
                var followBoard = await accountService.FollowBoardAsync(board);
                board.following = !string.IsNullOrEmpty((followBoard.Follow.board_id));
                Context.ShowMessage(board.following ? "关注成功" : "关注失败");
            }
        }
    }
}
