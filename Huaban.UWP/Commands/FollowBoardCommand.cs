using System;
using System.Windows.Input;

namespace Huaban.UWP.Commands
{
	using Models;
	using Services;
	using Api;
	using Base;
	public class FollowBoardCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public async void Execute(object parameter)
		{
			try
			{
				Board board = parameter as Board;

				string str = await ServiceLocator.Resolve<BoardAPI>().follow(board.board_id, !board.following);
				board.following = (str != "{}");
				ServiceLocator.Resolve<Context>()?.ShowTip(board.following ? "关注成功" : "已取消关注");
			}
			catch (Exception ex) { }
		}
	}
}
