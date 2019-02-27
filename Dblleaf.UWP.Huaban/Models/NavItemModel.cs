using System;
using Windows.UI.Xaml.Controls;

namespace Dblleaf.UWP.Huaban.Models
{
    public class NavItemModel : ObservableObject
    {
        private string _Label;
        public string Label
        {
            get { return _Label; }
            set { SetValue(ref _Label, value); }
        }
        public string Title { set; get; }

        public Symbol Symbol
        {
            set
            {
                char c = (char)value;
                SymbolChar = c;
            }
        }
        private char _SymbolChar;
        public char SymbolChar
        {
            get { return _SymbolChar; }
            set { SetValue(ref _SymbolChar, value); }
        }

        public string DestinationPage { get; set; }
        public object Arguments { get; set; }
        public bool Authorization { set; get; }

        private bool _Special;
        public bool Special
        {
            get { return _Special; }
            set { SetValue(ref _Special, value); }
        }

    }
}
