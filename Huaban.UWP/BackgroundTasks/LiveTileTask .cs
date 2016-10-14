using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Windows.Foundation;

namespace Huaban.UWP.BackgroundTasks
{
	public class LiveTileTask : IBackgroundTask
	{
		public async void Run(IBackgroundTaskInstance taskInstance)
		{
			var deferral = taskInstance.GetDeferral();
			await GetLatestPins();
			deferral.Complete();
		}

		private IAsyncOperation<string> GetLatestPins()
		{
			try
			{
				return AsyncInfo.Run(token => GetPins());
			}
			catch (Exception) { }
			return null;
		}

		private async Task<string> GetPins()
		{
			await Task.Delay(0);
			API.Current().PinAPI.get
			return null;
		}
	}
}
