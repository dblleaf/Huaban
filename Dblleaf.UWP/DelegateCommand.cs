using System;
using System.Windows.Input;

namespace Dblleaf.UWP
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object parameter)
        {
            return this.MyCanExecute == null ? true : this.MyCanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            try
            {
                this.MyExecute(parameter);
            }
            catch (Exception ex)
            {
            }
        }

        public Action<Object> MyExecute { get; set; }
        public Func<Object, bool> MyCanExecute { get; set; }

        public DelegateCommand(Action<Object> execute, Func<Object, bool> canExecute)
        {
            this.MyExecute = execute;
            this.MyCanExecute = canExecute;
        }
    }
}
