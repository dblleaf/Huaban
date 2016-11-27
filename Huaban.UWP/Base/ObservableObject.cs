using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Huaban.UWP.Base
{
	public abstract class ObservableObject : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (null != handler)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		protected bool SetValue<Entity>(ref Entity propertyValue, Entity newValue, [CallerMemberName]string propertyName = "")
		{
			propertyValue = newValue;
			NotifyPropertyChanged(propertyName);
			return object.Equals(propertyValue, newValue);
		}
	}
}
