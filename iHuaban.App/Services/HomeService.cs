using iHuaban.App.Helpers;
using iHuaban.App.Models;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public class HomeService : IHomeService
    {
        private IApiHttpHelper HttpHelper;
        public HomeService(IApiHttpHelper httpHelper)
        {
            this.HttpHelper = httpHelper;
        }

        public Home GetPagingHome(int page)
        {
            return GetPagingHomeAsync(page).Result;
        }

        public async Task<Home> GetPagingHomeAsync(int page)
        {
            if (page <= 0)
            {
                page = 1;
            }

            string url = $"?page={page}";
            string json = await HttpHelper.GetStringAsync(url);
            return Home.ParseHome(json);
        }
    }
}
