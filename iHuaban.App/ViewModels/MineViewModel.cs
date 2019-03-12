using iHuaban.App.Models;
using iHuaban.Core.Models;
using Windows.UI.Xaml;

namespace iHuaban.App.ViewModels
{
    public class MineViewModel : ViewModelBase
    {
        public LoginViewModel LoginViewModel { get; } = new LoginViewModel();
        public MineViewModel()
        {
            HasLogged = false;
            Me = new User
            {
                username = "碗大面宽",
                pin_count = 65,
                board_count = 2,
                avatar = new File
                {
                    id = 202148178,
                    key = "00b0c515f7d3f89bc96c8ad3f17fba334c23dc47f2b-D7MsIH",
                    width = "160",
                    height = "160"
                }
            };
        }
        public override string Icon => Constants.IconMine;
        public override string Title => Constants.TextMine;
        public override string TemplateName => Constants.TemplateMine;

        private User _Me;
        public User Me
        {
            set => SetValue(ref _Me, value);
            get => _Me;
        }
        private Visibility _MeVisibility;
        public Visibility MeVisibility
        {
            set => SetValue(ref _MeVisibility, value);
            get => _MeVisibility;
        }

        private Visibility _LoginVisibility;
        public Visibility LoginVisibility
        {
            set => SetValue(ref _LoginVisibility, value);
            get => _LoginVisibility;
        }

        public bool HasLogged
        {
            set
            {
                LoginVisibility = (value ? Visibility.Collapsed : Visibility.Visible);
                MeVisibility = (!value ? Visibility.Collapsed : Visibility.Visible);
            }
            get
            {
                return Me == null;
            }
        }
    }
}
