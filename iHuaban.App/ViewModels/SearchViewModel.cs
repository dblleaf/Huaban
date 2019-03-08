using iHuaban.App.Models;
using iHuaban.App.Services;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IServiceProvider = iHuaban.App.Services.IServiceProvider;

namespace iHuaban.App.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {
        private IServiceProvider ServiceProvider { get; set; }
        private IncrementalLoadingList<IModel> _Data;
        public IncrementalLoadingList<IModel> Data
        {
            get { return _Data; }
            set { SetValue(ref _Data, value); }
        }
        public SearchViewModel(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.Data = new IncrementalLoadingList<IModel>(GetData);
        }

        private async Task<IEnumerable<IModel>> GetData(uint startIndex, int page)
        {
            if (IsLoading)
            {
                return new List<IModel>();
            }
            IsLoading = true;
            try
            {
                var result = await ServiceProvider.GetAsync<IEnumerable<IModel>>("", 20, GetMaxPinId());

                if (result.Count() > 0)
                    Data.HasMore();
                else
                    Data.NoMore();

                return result;
            }
            catch
            {
            }
            finally
            {
                IsLoading = false;
            }

            return null;
        }

        private long GetMaxPinId()
        {
            if (Data?.Count > 0 && long.TryParse(Data?[Data.Count - 1].KeyId, out long maxId))
            {
                return maxId;
            }
            else
            {
                return 0;
            }
        }
    }
}
