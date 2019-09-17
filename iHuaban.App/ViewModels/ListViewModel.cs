using iHuaban.App.Helpers;
using iHuaban.App.Models;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace iHuaban.App.ViewModels
{
    public class ListViewModel<T> : ViewModelBase
        where T : IModel
    {
        internal IApiHttpHelper HttpHelper { get; set; }
        internal Func<string, IEnumerable<T>> Converter { get; set; }
        public DataType DataType { get; private set; } = new DataType();
        public ListViewModel(
            DataType dataType,
            IApiHttpHelper httpHelper,
            Func<string, IEnumerable<T>> converter)
        {
            this.DataType = dataType;
            this.HttpHelper = httpHelper;
            this.Converter = converter;
            this.Data = new IncrementalLoadingList<T>(GetData);
        }

        public IncrementalLoadingList<T> Data { private set; get; }

        private async Task<IEnumerable<T>> GetData(uint startIndex, int page)
        {
            if (IsLoading)
            {
                return new List<T>();
            }
            IsLoading = true;
            try
            {
                string query = "?limit=20";
                var max = GetMaxPinId();
                if (max > 0)
                {
                    query += $"&max={max}";
                }
                var url = $"{ this.DataType.BaseUrl.Trim('/')}{query}";
                var json = await HttpHelper.GetStringAsync(url);
                var result = this.Converter(json);
                if (result.Count() == 0)
                {
                    NoMoreVisibility = Visibility.Visible;
                    Data.NoMore();
                }
                else
                {
                    NoMoreVisibility = Visibility.Collapsed;
                    Data.HasMore();
                }

                return result;
            }
            catch { }
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
