using iHuaban.App.Models;
using iHuaban.Core.Helpers;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public class HomeService : IHomeService
    {
        private IHttpHelper HttpHelper;
        public HomeService(IHttpHelper httpHelper)
        {
            this.HttpHelper = httpHelper;
        }

        public Home GetPagingHome(int page)
        {
            return GetPagingHomeAsync(page).Result;
        }

        public async Task<Home> GetPagingHomeAsync(int page)
        {
            string url = Constants.UrlBase;
            if (page > 0)
            {
                url += $"discovery/?page={page}";
            }
            string json = await HttpHelper.GetStringAsync(url);
            return Home.ParseHome(json);
        }
    }
}
