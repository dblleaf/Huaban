using System;
using System.Windows.Input;

namespace Huaban.UWP.Commands
{
	using Base;
	using Services;
	public class ToBoardDetailCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			try
			{
				ServiceLocator.Resolve<NavigationService>().NavigateTo("BoardDetail", parameter);

			}
			catch (Exception ex)
			{

			}
		}
	}
}
