using iHuaban.Core.Helpers;

namespace iHuaban.App.Helpers
{
    public class ApiHttpHelper : HttpHelper, IApiHttpHelper
    {
        public override string BaseUrl => "https://api.huaban.com";
    }
}
