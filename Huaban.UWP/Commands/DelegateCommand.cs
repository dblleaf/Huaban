using System;
using System.Windows.Input;

namespace Huaban.UWP.Commands
{
	public class DelegateCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;
		/// <summary>
		/// 需要手动触发属性改变事件
		/// </summary>
		public void RaiseCanExecuteChanged()
		{
			if (CanExecuteChanged != null)
			{
				CanExecuteChanged(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// 决定当前绑定的Command能否被执行
		/// true：可以被执行
		/// false：不能被执行
		/// </summary>
		/// <param name="parameter">不是必须的，可以依据情况来决定，或者重写一个对应的无参函数</param>
		/// <returns></returns>
		public bool CanExecute(object parameter)
		{
			return this.MyCanExecute == null ? true : this.MyCanExecute(parameter);
		}

		/// <summary>
		/// 用于执行对应的命令，只有在CanExecute可以返回true的情况下才可以被执行
		/// </summary>
		/// <param name="parameter"></param>
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

		/// <summary>
		/// 
		/// </summary>
		public Action<Object> MyExecute { get; set; }
		public Func<Object, bool> MyCanExecute { get; set; }

		/// <summary>
		/// 构造函数，用于初始化
		/// </summary>
		/// <param name="execute"></param>
		/// <param name="canExecute"></param>
		public DelegateCommand(Action<Object> execute, Func<Object, bool> canExecute)
		{
			this.MyExecute = execute;
			this.MyCanExecute = canExecute;
		}
	}
}
