using Dblleaf.UWP.Huaban.Models;
using Dblleaf.UWP.Huaban.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Dblleaf.UWP.Huaban.ViewModels
{
    public class HBViewModel : ViewModelBase
    {
        public HBViewModel(Context context)
        {
            Context = context;
            LeftHeaderVisibility = Visibility.Visible;
        }
        #region Properties

        protected Context Context { get; private set; }
        public bool IsLogin { get { return Context.IsLogin; } }

        private Visibility _LeftHeaderVisibility;
        public Visibility LeftHeaderVisibility
        {
            get { return _LeftHeaderVisibility; }
            protected set { SetValue(ref _LeftHeaderVisibility, value); }
        }

        private string _QuickBoardName;
        public string QuickBoardName
        {
            get { return _QuickBoardName; }
            set { SetValue(ref _QuickBoardName, value); }
        }

        private bool _CanQuick;
        public bool CanQuick
        {
            get { return _CanQuick; }
            set { SetValue(ref _CanQuick, value); }
        }

        public static event EventHandler<EventArgs> QuickBoardChanged;
        private static Board _QuickBoard;
        internal static Board QuickBoard
        {
            get { return _QuickBoard; }
            set
            {
                _QuickBoard = value;
                QuickBoardChanged?.Invoke(null, null);
            }
        }

        public string TargetName { set; get; }

        #endregion


        #region Methods

        public override Size ArrangeOverride(Size finalSize)
        {
            if (Window.Current.Bounds.Width >= 720)
                LeftHeaderVisibility = Visibility.Collapsed;
            else
                LeftHeaderVisibility = Visibility.Visible;
            return finalSize;
        }

        protected void InitQuickBoard()
        {
            QuickBoardName = QuickBoard?.title;
            CanQuick = QuickBoard != null;
        }

        #endregion
    }
}
