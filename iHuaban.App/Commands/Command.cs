using iHuaban.App.Models;
using System;
using System.Windows.Input;

namespace iHuaban.App.Commands
{
    public abstract class Command : ICommand
    {
        protected Context Context { get; private set; }
        public event EventHandler CanExecuteChanged;
        public Command(Context context)
        {
            this.Context = context;
        }

        public virtual void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public abstract void Execute(object parameter);
    }
}
