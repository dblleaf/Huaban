using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.UI.Xaml;
namespace Huaban.UWP.Base
{
	using Controls;
	using Services;
	public abstract class ViewModelBase : ObservableObject
	{
		public ViewModelBase()
		{
		}

		#region base
		protected virtual void LockBack()
		{
			IsLockBack = true;
		}
		protected virtual void UnlockBack()
		{
			IsLockBack = false;
		}
		public virtual bool IsLockBack { set; get; }

		public virtual void OnNavigatedTo(HBNavigationEventArgs e) { }

		public virtual void OnNavigatedFrom(HBNavigationEventArgs e)
		{
			_LoadingCount = 0;
			NotifyPropertyChanged("IsLoading");
		}

		public virtual bool OnNavigatingFrom(HBNavigatingCancelEventArgs e)
		{
			var ret = e.Cancel = IsLockBack;
			UnlockBack();
			return ret;
		}
		public virtual Size ArrangeOverride(Size finalSize)
		{
			return finalSize;
		}


		public bool IsInited { get; private set; } = false;
		public virtual void Inited()
		{
			IsInited = true;
		}
		#endregion

		#region Properties

		private string _Title;
		public string Title { get { return _Title; } set { SetValue(ref _Title, value); } }

		#region loading
		protected Action Loading;
		private int _LoadingCount;
		private bool _IsLoading;
		public bool IsLoading
		{
			get
			{
				return _LoadingCount > 0;
			}
			set
			{
				if (value)
					_LoadingCount++;
				else
					_LoadingCount--;

				NotifyPropertyChanged();

				Loading?.Invoke();
			}
		}
		#endregion

		#endregion

	}
}
