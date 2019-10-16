using iHuaban.App.Services;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace iHuaban.App.Views.Content
{
    /// <summary>
    /// This UserControl is referenced at https://www.cnblogs.com/MzwCat/p/7748033.html.
    /// Thanks @Zhiwu Mao, https://github.com/creatorMao.
    /// </summary>
    public sealed partial class PopupMessage : UserControl
    {
        private Popup popup = null;
        public PopupMessage()
        {
            this.InitializeComponent();

            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;

            popup = new Popup();
            popup.Child = this;
            this.PopupOut.Completed += PopupOutCompleted;
            this.PopupIn.Completed += PopupInCompleted;
        }

        private void OnShow(string message)
        {
            popup.RequestedTheme = ThemeService.Theme;
            this.PopupContent.Text = message;
            popup.IsOpen = true;
            this.PopupIn.Begin();
        }

        public async void PopupInCompleted(object sender, object e)
        {
            await Task.Delay(1000);
            this.PopupOut.Begin();
        }

        public void PopupOutCompleted(object sender, object e)
        {
            popup.IsOpen = false;
        }

        private static PopupMessage instance;
        public static void ShowMessage(string message)
        {
            instance = instance ?? new PopupMessage();
            instance.OnShow(message);
        }
    }
}
