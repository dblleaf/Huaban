using System;
using System.Windows.Input;

namespace Huaban.UWP.Commands
{
	using Models;
	public class FollowUserCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public async void Execute(object parameter)
		{
			User user = parameter as User;
			string str = await App.AppContext.API.UserAPI.follow(user?.user_id, !user.following);
			user.following = !user.following;
			App.AppContext.ShowTip(user.following ? "关注成功" : "已取消关注");
		}
	}
}
