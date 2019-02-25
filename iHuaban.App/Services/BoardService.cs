using iHuaban.App.Models;
using iHuaban.Core.Helpers;

namespace iHuaban.App.Services
{
    public class BoardService : Service<BoardCollection>, IBoardService
    {
        public BoardService(IHttpHelper httpHelper) : base(httpHelper)
        {
        }
    }
}
