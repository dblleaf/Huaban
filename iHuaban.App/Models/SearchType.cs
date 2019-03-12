namespace iHuaban.App.Models
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class DataType
    {
        public DataType(string type, string url, Func<string, Task<IEnumerable<IModel>>> dataLoaderAsync)
        {
            this.Type = type;
            this.Url = url;
            this.DataLoaderAsync = dataLoaderAsync;
        }
        public string Type { get; set; }
        public string Url { get; set; }
        public Func<string, Task<IEnumerable<IModel>>> DataLoaderAsync { get; set; }
    }
}
