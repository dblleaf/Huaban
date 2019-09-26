using iHuaban.Core.Helpers;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iHuaban.App.Models
{
    public class DataItem<T> where T : IModel
    {
        public Func<DataItem<T>, string> UrlAction { get; set; }
        public string Url { get; set; }
        private bool IsLoading;
        private IHttpHelper httpHelper;
        public DataItem(IHttpHelper httpHelper)
        {
            this.httpHelper = httpHelper;
            Data = new IncrementalLoadingList<T>(GetDataAsync);
        }

        public IncrementalLoadingList<T> Data { get; set; }

        private async Task<IEnumerable<T>> GetDataAsync(uint startIndex, int page)
        {
            if (IsLoading )
            {
                return new List<T>();
            }
            IsLoading = true;
            try
            {
                var url = Url;
                var result = await httpHelper.GetAsync<List<T>>(url);

                if (result.Count() == 0)
                {
                    Data.NoMore();
                }
                else
                {
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
    }
}
