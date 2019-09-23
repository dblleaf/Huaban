using iHuaban.App.Commands;
using System.Windows.Input;

namespace iHuaban.App.Locators
{
    public class CommandLocator
    {
        public ICommand DownloadCommand => UnityConfig.ResolveObject<DownloadCommand>();

        public ICommand FollowBoardCommand => UnityConfig.ResolveObject<FollowBoardCommand>();

        public ICommand FollowUserCommand => UnityConfig.ResolveObject<FollowUserCommand>();

        public ICommand LikePinCommand => UnityConfig.ResolveObject<LikePinCommand>();

        public ICommand PinCommand => UnityConfig.ResolveObject<PinCommand>();

        public ICommand QuickPinCommand => UnityConfig.ResolveObject<QuickPinCommand>();

        public ICommand ToBoardCommand => UnityConfig.ResolveObject<ToBoardCommand>();

        public ICommand ToPinCommand => UnityConfig.ResolveObject<ToPinCommand>();

        public ICommand ToUserCommand => UnityConfig.ResolveObject<ToUserCommand>();
    }
}
