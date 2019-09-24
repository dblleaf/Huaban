using iHuaban.App.Models;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public interface IAccountService
    {
        Task<AuthResult> LoginAsync(string userName, string password);
        Task LoadMeAsync();
        Task<PinResult> PickPin(Pin pin, string boardId);
        Task<LikeResult> LikePin(Pin pin);
        Task<string> UnLikePin(Pin pin);
        Task<FollowBoardResult> FollowBoard(Board board);
        Task<string> UnFollowBoard(Board board);
        Task<string> FollowUser(User user);
        Task<string> UnFollowUser(User user);
        Task<string> CreateBoard(Board board, string category);
    }
}
