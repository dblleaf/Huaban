using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Huaban.UWP.Commands
{
	using Models;
	using ViewModels;
	public class PinImageLoadingCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var model = parameter as PinListViewModel;
			if (model != null)
				model.IsLoading = true;

		}
	}
}
