namespace iHuaban.App.Models
{
    using iHuaban.App.TemplateSelectors;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Windows.UI.Xaml.Controls;

    public class DataType
    {
        public string Type { get; set; }
        public string Url { get; set; }
        public Func<string, Task<IEnumerable<IModel>>> DataLoaderAsync { get; set; }
        public string ScaleSize { get; set; } = "300:300";
        public decimal CellMinWidth { get; set; } = 236;
    }
}
