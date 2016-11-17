using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Huaban.UWP.ViewModels
{
	using Base;
	using Models;
	public class MessageViewModel : HBViewModel
	{
		public MessageViewModel(Context context)
			: base(context)
		{
			Title = "消息";
			PinListVM = new PinListViewModel(context, GetData);
			
		}

		#region Properties

		public PinListViewModel PinListVM { get; set; }

		#endregion

		#region Commands


		#endregion

		#region Methods

		private async Task<IEnumerable<Pin>> GetData(uint startIndex, int page)
		{
			PinListVM.PinList.HasMore();

			IsLoading = true;
			try
			{
				var list = await Context.API.CategoryAPI.GetCategoryPinList("/all/", 20, PinListVM.GetMaxPinID());
				if (list?.Count == 0)
					PinListVM.PinList.NoMore();
				else
					PinListVM.PinList.HasMore();
				return list;
			}
			catch (Exception ex)
			{
			}
			finally
			{
				IsLoading = false;
			}
			return null;
		}

		#endregion
	}
}
