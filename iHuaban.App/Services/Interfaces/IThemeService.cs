using Windows.UI.Xaml;

namespace iHuaban.App.Services
{
    public interface IThemeService
    {
        ElementTheme RequestTheme { get; }
        void SetTheme(ElementTheme theme);
        void LoadTheme();
        void SetTheme();
    }
}
