using System;
using System.Windows.Input;

namespace Huaban.UWP.Commands
{
	using Services;
	public class ToUserCommand : ICommand
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
				ServiceLocator.Resolve<NavigationService>().NavigateTo("User", parameter);

			}
			catch (Exception ex)
			{

			}
		}
	}
}
