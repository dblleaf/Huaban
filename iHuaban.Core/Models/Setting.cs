using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace iHuaban.Core.Models
{
    public class Setting : ObservableObject
    {
        private Setting()
        {
        }

        private string _SavePath;
        public string SavePath
        {
            get { return _SavePath; }
            set { SetValue(ref _SavePath, value); }
        }

        private static Setting setting;
        public static Setting Instance()
        {
            return setting ?? (setting = new Setting());
        }

        public async Task InitAsync()
        {
            await Task.FromResult(0);
        }

        private bool _WindowActived;
        public bool WindowActived
        {
            get { return _WindowActived; }
            set { SetValue(ref _WindowActived, value); }
        }

        private ElementTheme _RequestedTheme;
        public ElementTheme RequestedTheme
        {
            get { return _RequestedTheme; }
            set { SetValue(ref _RequestedTheme, value); }
        }

    }
}