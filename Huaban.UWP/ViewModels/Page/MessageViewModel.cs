using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace Huaban.UWP.ViewModels
{
    using Base;
    using Models;
    using Api;
    public class MessageViewModel : HBViewModel
    {
        public MessageViewModel(Context context)
            : base(context)
        {
            Title = "消息";
            PinListVM = new PinListViewModel(context, GetData);
        }

        #region Properties
        [Dependency]
        public CategoryAPI CategoryAPI { set; get; }
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
                var list = await CategoryAPI.GetCategoryPinList("/all/", 20, PinListVM.GetMaxPinID());
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
