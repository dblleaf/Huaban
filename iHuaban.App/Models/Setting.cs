using iHuaban.Core.Models;
using System.ComponentModel;
using System.Threading.Tasks;

namespace iHuaban.App.Models
{
    public class Setting : ObservableObject
    {
        private Setting()
        {
            this.PropertyChanged += Setting_PropertyChanged;
        }

        private void Setting_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private bool _DarkMode;
        public bool DarkMode
        {
            get { return _DarkMode; }
            set { SetValue(ref _DarkMode, value); }
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
    }
}
