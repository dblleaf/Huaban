using System.Threading.Tasks;
using iHuaban.App.Models;
using iHuaban.Core.Helpers;

namespace iHuaban.App.Services
{
    public class BoardService : IBoardService
    {
        public BoardService(IHttpHelper httpHelper)
        {
        }

        public BoardCollection Get(string uri, int limit = 0, long max = 0)
        {
            throw new System.NotImplementedException();
        }

        public Task<BoardCollection> GetAsync(string uri, int limit = 0, long max = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}
