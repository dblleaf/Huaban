using System;
using System.Windows.Input;

namespace Huaban.UWP.Commands
{
	using Services;
	public class ToBoardPinsCommand : ICommand
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
				ServiceLocator.Resolve<NavigationService>().NavigateTo("BoardPins", parameter);
			}
			catch (Exception ex)
			{

			}
		}
	}
}
