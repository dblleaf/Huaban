using iHuaban.Core.Models;

namespace iHuaban.App.Models
{
    public class PinDownloadItem: ObservableObject
    {
        public Pin Pin { get; set; }
        public long TotalSize { get; set; }
        public long DownSize { get; set; }
    }
}
