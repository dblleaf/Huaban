using iHuaban.App.Models;
using iHuaban.App.Views.Content;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.Commands
{
    public class ToPinCommand : Command
    {
        public ToPinCommand(Context context)
            : base(context) { }

        public override void Execute(object parameter)
        {
            if (parameter is ListViewBase listView)
            {
                ImageViewer.Show(listView);
            }
        }
    }
}
