using iHuaban.App.Services;
using iHuaban.Core.Commands;
using iHuaban.Core.Models;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace iHuaban.App.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        private IThemeService themeService;
        public SettingViewModel(IThemeService themeService)
        {
            this.themeService = themeService;
            this.DarkMode = themeService.RequestTheme == ElementTheme.Dark;
            this.PropertyChanged += SettingViewModel_PropertyChanged;
        }

        private void SettingViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DarkMode))
            {
                ElementTheme theme = DarkMode ? ElementTheme.Dark : ElementTheme.Light;
                this.themeService.SetTheme(theme);
            }
        }

        private bool _DarkMode;
        public bool DarkMode
        {
            get { return _DarkMode; }
            set { SetValue(ref _DarkMode, value); }
        }

        private DelegateCommand logoutCommand;
        public DelegateCommand LogoutCommand
        {
            get
            {
                return logoutCommand ?? (logoutCommand = new DelegateCommand(
                    async o =>
                    {
                        await Task.Delay(0);
                    },
                    o => true)
                );
            }
        }
    }
}
