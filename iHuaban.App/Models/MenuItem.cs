using iHuaban.Core.Models;
using System;

namespace iHuaban.App.Models
{
    public class MenuItem : ObservableObject
    {
        public string Title { get; set; }
        private string _Icon;
        public string Icon
        {
            get { return _Icon; }
            set { SetValue(ref _Icon, value); }
        }

        public Type Type { get; set; }
    }
}
