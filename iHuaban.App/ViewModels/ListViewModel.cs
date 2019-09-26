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
    public enum ListViewModelType
    {
        Max,
        Page,
    }

    public class ListViewModel<T> : ViewModelBase
        where T : IModel
    {
        internal IApiHttpHelper HttpHelper { get; set; }
        internal Func<string, IEnumerable<T>> Converter { get; set; }
        internal ListViewModelType Type { get; set; } = ListViewModelType.Max;
        public DataType DataType { get; private set; } = new DataType();
        private Func<T, string> feedKeyfunc;
        public ListViewModel(
            DataType dataType,
            IApiHttpHelper httpHelper,
            Func<string, IEnumerable<T>> converter,
            Func<T, string> feedKeyfunc = null,
            ListViewModelType type = ListViewModelType.Max)
        {
            this.DataType = dataType;
            this.HttpHelper = httpHelper;
            this.Converter = converter;
            this.feedKeyfunc = feedKeyfunc;
            this.Type = type;
            this.Data = new IncrementalLoadingList<T>(GetData);
        }

        public IncrementalLoadingList<T> Data { private set; get; }

        private int currentPage = 0;
        private async Task<IEnumerable<T>> GetData(uint startIndex, int page)
        {
            if (IsLoading)
            {
                return new List<T>();
            }
            IsLoading = true;
            try
            {
                var query = string.Empty;
                if (this.Type == ListViewModelType.Max)
                {
                    query = "?limit=20";
                    var max = GetMaxKeyId();
                    if (max > 0)
                    {
                        query += $"&max={max}";
                    }

                }
                else if (this.Type == ListViewModelType.Page)
                {
                    query = $"?page={++currentPage}&per_page=20";
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

        private long GetMaxKeyId()
        {
            if (feedKeyfunc == null)
            {
                feedKeyfunc = d => d.KeyId;
            }
            if (Data?.Count > 0 && long.TryParse(feedKeyfunc(Data[Data.Count - 1]), out long maxId))
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
