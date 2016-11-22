using System;
using Windows.Foundation;

namespace Huaban.UWP.Base
{
	using Controls;
	public abstract class ViewModelBase : ObservableObject, IDisposable
	{
		public ViewModelBase()
		{
		}

		#region base

		public virtual void OnNavigatedTo(HBNavigationEventArgs e) { }

		public virtual void OnNavigatedFrom(HBNavigationEventArgs e)
		{
			_LoadingCount = 0;
			NotifyPropertyChanged("IsLoading");
		}

		public virtual bool OnNavigatingFrom(HBNavigatingCancelEventArgs e)
		{
			Dispose();
			return true;
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

		public virtual void Dispose()
		{
			GC.SuppressFinalize(this);
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
		}
		#endregion

		#region Properties

		private string _Title;
		public string Title { get { return _Title; } set { SetValue(ref _Title, value); } }

		#region loading
		protected Action Loading;
		protected int _LoadingCount;
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
