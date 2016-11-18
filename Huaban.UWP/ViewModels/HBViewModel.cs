using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Huaban.UWP.ViewModels
{
    using Base;
    using Models;
    using Commands;
    using Services;

    public class HBViewModel : ViewModelBase
    {
        protected NavigationService NavigationService { get; set; }
        public HBViewModel(Context context, NavigationService ns)
        {
            Context = context;
            NavigationService = ns;
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

        #region Commands

        private DelegateCommand _ToBoardPinsCommand;
        public DelegateCommand ToBoardPinsCommand
        {
            get
            {
                return _ToBoardPinsCommand ?? (_ToBoardPinsCommand = new DelegateCommand(
                    (Object obj) =>
                    {
                        var args = obj as ItemClickEventArgs;
                        var item = obj as Board;
                        if (args == null && item == null)
                            return;

                        if (args != null)
                            item = args.ClickedItem as Board;
                        if (item != null)
                        {
                            NavigationService.NavigateTo("BoardPins", item);
                        }
                    },
                    (Object obj) => !IsLoading)
                );
            }
        }

        //ToPinDetailCommand
        private DelegateCommand _ToPinDetailCommand;
        public DelegateCommand ToPinDetailCommand
        {
            get
            {
                return _ToPinDetailCommand ?? (_ToPinDetailCommand = new DelegateCommand(
                    (Object obj) =>
                    {
                        var args = obj as ItemClickEventArgs;
                        var item = obj as Pin;
                        if (args == null && item == null)
                            return;

                        if (args != null)
                            item = args.ClickedItem as Pin;

                        NavigationService.NavigateTo("PinDetail", item);
                    },
                    (Object obj) => !IsLoading)
                );
            }
        }
        //ToImageViewCommand
        private DelegateCommand _ToImageViewCommand;
        public DelegateCommand ToImageViewCommand
        {
            get
            {
                return _ToImageViewCommand ?? (_ToImageViewCommand = new DelegateCommand(
                    (Object obj) =>
                    {
                        PinListViewModel model = obj as PinListViewModel;

                        if (model == null)
                            return;

                        NavigationService.NavigateTo("Image", model);
                    },
                    (Object obj) => !IsLoading)
                );
            }
        }

        //ToPinDetailCommand
        private DelegateCommand _ToUserPageCommand;
        public DelegateCommand ToUserPageCommand
        {
            get
            {
                return _ToUserPageCommand ?? (_ToUserPageCommand = new DelegateCommand(
                    (Object obj) =>
                    {
                        var args = obj as ItemClickEventArgs;
                        var item = obj as User;
                        if (args == null && item == null)
                            return;

                        if (args != null)
                            item = args.ClickedItem as User;

                        if (item != null)
                        {
                            NavigationService.NavigateTo("User", item);
                        }
                    },
                    (Object obj) => !IsLoading)
                );
            }
        }
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