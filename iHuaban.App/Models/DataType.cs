namespace iHuaban.App.Models
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class DataType
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string BaseUrl { get; set; }
        public long Badge { get; set; }
        public Func<string, Task<IEnumerable<IModel>>> DataLoaderAsync { get; set; }
        public Func<DataType, string> UrlAction { get; set; }
        public string ScaleSize { get; set; } = "300:300";
        public decimal CellMinWidth { get; set; } = 236;

        public string GetUrl()
        {
            if (UrlAction != null)
            {
                return UrlAction.Invoke(this);
            }

            return BaseUrl;
        }
    }
}
