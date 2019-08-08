using iHuaban.App.Models;
using iHuaban.Core.Helpers;
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
        internal string BaseUrl { get; set; }
        internal IHttpHelper HttpHelper { get; set; }
        internal Func<string, IEnumerable<T>> Converter { get; set; }
        public ListViewModel() { }

        public ListViewModel(string baseUrl, string templateName, IHttpHelper httpHelper, Func<string, IEnumerable<T>> converter)
        {
            this.BaseUrl = baseUrl;
            this.HttpHelper = httpHelper;
            this.Converter = converter;
            this.TemplateName = templateName;
            this.Data = new IncrementalLoadingList<T>(GetData);
        }

        public void SetBaseUrl(string baseUrl)
        {
            this.BaseUrl = baseUrl;
            this.Data.Clear();
            this.Data.HasMore();
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
                var url = $"{ this.BaseUrl.Trim('/')}{query}";
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
