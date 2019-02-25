using iHuaban.App.Models;
using iHuaban.Core.Helpers;

namespace iHuaban.App.Services
{
    public class PinService : Service<PinCollection>, IPinService
    {
        public PinService(IHttpHelper httpHelper) : base(httpHelper)
        {
        }
    }
}
