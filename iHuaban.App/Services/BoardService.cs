using iHuaban.App.Models;
using iHuaban.Core;
using iHuaban.Core.Helpers;

namespace iHuaban.App.Services
{
    public class BoardService : PinsResultService<Board>
    {
        public BoardService(string boardName, HttpHelper httpHelper)
            : base($"{Constants.ApiBoardsName}/{boardName}/", httpHelper)
        { }
    }
}
