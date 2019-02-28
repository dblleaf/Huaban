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
            string url = Constants.ApiBase;
            if (page > 0)
            {
                url += $"?page={page}";
            }
            string json = await HttpHelper.GetAsync(url);
            return Home.ParseHome(json);
        }
    }
}
