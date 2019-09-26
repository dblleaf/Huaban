using iHuaban.App.Models;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public interface IAccountService
    {
        Task<AuthResult> LoginAsync(string userName, string password);
        Task<User> GetMeAsync();
        Task<PinResult> PickPinAsync(Pin pin, string boardId);
        Task<LikeResult> LikePinAsync(Pin pin);
        Task<string> UnLikePinAsync(Pin pin);
        Task<FollowBoardResult> FollowBoardAsync(Board board);
        Task<string> UnFollowBoardAsync(Board board);
        Task<string> FollowUserAsync(User user);
        Task<string> UnFollowUserAsync(User user);
        Task<string> CreateBoardAsync(Board board, string category);
        Task<BoardCollection> GetLastBoardsAsync();
    }
}
