using iHuaban.Core;

namespace iHuaban.App.Services
{
    public class BoardService : PinsResultService
    {
        public BoardService(string boardName) : base(boardName)
        { }

        public override string GetApiUrl()
        {
            return Constants.ApiBoards;
        }

        public override string GetApiPinsUrl()
        {
            return GetApiUrl() + ResourceName + "/pins";
        }
    }
}
