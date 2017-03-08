using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Core;

namespace Huaban.UWP.ViewModels
{
    using Base;
    using Commands;
    using Services;
    using Models;
    using Api;
    public delegate void SelectedChangedHandler(Pin args);
    public class PinListViewModel : HBViewModel
    {
        private PinAPI PinAPI { set; get; }
        private BoardAPI BoardAPI { set; get; }
        public PinListViewModel(Context context, Func<uint, int, Task<IEnumerable<Pin>>> _func)
            : base(context)
        {
            PinAPI = ServiceLocator.Resolve<PinAPI>();
            BoardAPI = ServiceLocator.Resolve<BoardAPI>();
            PinList = new IncrementalLoadingList<Pin>(_func);
            SelecterVisibility = Visibility.Collapsed;

            QuickBoardChanged += (s, e) =>
            {
                InitQuickBoard();
            };

            InitQuickBoard();
        }

        public event SelectedChangedHandler OnSelectedChanged;

        #region Properties


        private double _ColumnWidth;
        public double ColumnWidth
        {
            get { return _ColumnWidth; }
            set { SetValue(ref _ColumnWidth, value); }
        }

        private int _ColumnCount;
        public int ColumnCount
        {
            get { return _ColumnCount; }
            set { SetValue(ref _ColumnCount, value); }
        }

        private IncrementalLoadingList<Pin> _PinList;
        public IncrementalLoadingList<Pin> PinList
        {
            get { return _PinList; }
            set { SetValue(ref _PinList, value); }
        }

        public int Count
        {
            get { return PinList.Count; }
        }

        private Pin _SelectedItem;
        public Pin SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                SetValue(ref _SelectedItem, value);
                OnSelectedChanged?.Invoke(value);
            }
        }

        private string _NewBoardName;
        public string NewBoardName
        {
            get { return _NewBoardName; }
            set { SetValue(ref _NewBoardName, value); }
        }

        private Visibility _SelecterVisibility;
        public Visibility SelecterVisibility
        {
            get { return _SelecterVisibility; }
            set { SetValue(ref _SelecterVisibility, value); }
        }

        private int _CurrentBoardIndex;
        public int CurrentBoardIndex
        {
            get { return _CurrentBoardIndex; }
            set { SetValue(ref _CurrentBoardIndex, value); }
        }

        public IncrementalLoadingList<Board> BoardList { get { return Context.BoardListVM?.BoardList; } }

        public Setting Setting { private set; get; } = Setting.Current;

        #endregion

        #region Commands

        //喜欢/取消喜欢
        private DelegateCommand _LikeCommand;
        public DelegateCommand LikeCommand
        {
            get
            {
                return _LikeCommand ?? (_LikeCommand = new DelegateCommand(
                    async o =>
                    {
                        if (!IsLogin)
                        {
                            Context.ShowTip("请先登录");
                            return;
                        }
                        var args = o as ItemClickEventArgs;
                        var item = o as Pin;
                        if (args == null && item == null)
                            return;

                        if (args != null)
                            item = args.ClickedItem as Pin;

                        string str = await PinAPI.Like(item.pin_id, !item.liked);

                        item.liked = (str != "{}");

                        Context.ShowTip(item.liked ? "已设置为喜欢" : "已取消喜欢");

                    }, o => true)
                );
            }
        }

        private DelegateCommand _NewBoardCommand;
        public DelegateCommand NewBoardCommand
        {
            get
            {
                return _NewBoardCommand ?? (_NewBoardCommand = new DelegateCommand(
                async o =>
                {
                    if (!IsLogin)
                    {
                        Context.ShowTip("请先登录");
                        return;
                    }

                    if (string.IsNullOrEmpty(NewBoardName))
                        return;
                    string boardName = NewBoardName;
                    NewBoardName = "";
                    var board = await BoardAPI.add(boardName);

                    if (board != null)
                    {
                        var list = Context.BoardListVM.BoardList;
                        list.Add(board);
                        var pin = await PinAPI.Pin(SelectedItem.pin_id, board.board_id, SelectedItem.raw_text);
                        board.pins.Add(pin);
                        board.cover = pin;
                        Context.ShowTip($"采集到了画板：{board.title}");
                        QuickBoard = board;
                        HideSelectCommand.Execute(null);

                    }
                }, o => true));
            }
        }

        private DelegateCommand _BoardKeyDownCommand;
        public DelegateCommand BoardKeyDownCommand
        {
            get
            {
                return _BoardKeyDownCommand ?? (_BoardKeyDownCommand = new DelegateCommand(
                o =>
                {
                    if (!IsLogin)
                    {
                        Context.ShowTip("请先登录");
                        return;
                    }

                    var e = o as KeyRoutedEventArgs;

                    if (e?.Key == Windows.System.VirtualKey.Enter)
                    {
                        var txt = e.OriginalSource as TextBox;
                        NewBoardName = txt.Text;
                        NewBoardCommand.Execute(e.OriginalSource);
                    }

                }, o => true));
            }
        }

        private DelegateCommand _SelectBoardCommand;
        public DelegateCommand SelectBoardCommand
        {
            get
            {
                return _SelectBoardCommand ?? (_SelectBoardCommand = new DelegateCommand(
                async o =>
                {
                    var args = o as ItemClickEventArgs;
                    var item = o as Board;
                    if (args == null && item == null)
                        return;

                    if (args != null)
                        item = args.ClickedItem as Board;

                    var pin = await PinAPI.Pin(SelectedItem.pin_id, item.board_id, SelectedItem.raw_text);
                    if (item.cover == null)
                        item.cover = pin;
                    Context.ShowTip($"采集到了画板：{item.title}");
                    QuickBoard = item;
                    HideSelectCommand.Execute(null);
                }, o => true));
            }
        }

        //快速采集
        private DelegateCommand _QuickPinCommand;
        public DelegateCommand QuickPinCommand
        {
            get
            {
                return _QuickPinCommand ?? (_QuickPinCommand = new DelegateCommand(
                o =>
                {
                    if (!IsLogin)
                    {
                        Context.ShowTip("请先登录");
                        return;
                    }
                    var args = o as ItemClickEventArgs;
                    var item = o as Pin;
                    if (args == null && item == null)
                        return;

                    if (args != null)
                        item = args.ClickedItem as Pin;
                    SelectedItem = item;
                    if (QuickBoard == null)
                    {
                        Context.ShowTip("没有快速采集的画板");
                        return;
                    }
                    SelectBoardCommand.Execute(QuickBoard);
                }, o => true));
            }
        }

        //弹出选择采集到画板
        private DelegateCommand _ShowSelectCommand;
        public DelegateCommand ShowSelectCommand
        {
            get
            {
                return _ShowSelectCommand ?? (_ShowSelectCommand = new DelegateCommand(
                o =>
                {
                    if (!IsLogin)
                    {
                        Context.ShowTip("请先登录");
                        return;
                    }
                    var args = o as ItemClickEventArgs;
                    var item = o as Pin;
                    if (args == null && item == null)
                        return;

                    if (args != null)
                        item = args.ClickedItem as Pin;
                    SelectedItem = item;
                    SelecterVisibility = Visibility.Visible;
                    NavigationService.BackEvent += NavigationService_BackEvent;
                    CurrentBoardIndex = -1;

                }, o => true));
            }
        }

        //隐藏选择采集到画板
        private DelegateCommand _HideSelectCommand;
        public DelegateCommand HideSelectCommand
        {
            get
            {
                return _HideSelectCommand ?? (_HideSelectCommand = new DelegateCommand(
                o =>
                {
                    SelecterVisibility = Visibility.Collapsed;
                    NavigationService.BackEvent -= NavigationService_BackEvent;
                }, o => true));
            }
        }

        #endregion

        #region Methods

        public async Task ClearAndReload()
        {
            await PinList.ClearAndReload();
        }

        public void Clear()
        {
            PinList.Clear();
        }

        public long GetMaxPinID()
        {
            long max = 0;
            if (Count > 0)
                max = Convert.ToInt64(PinList[Count - 1].pin_id);
            return max;
        }
        public long GetMaxSeq()
        {
            long max = 0;
            if (Count > 0)
                max = Convert.ToInt64(PinList[Count - 1].seq);
            return max;
        }

        private void NavigationService_BackEvent(object sender, BackRequestedEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                HideSelectCommand.Execute(null);
            }
        }

        public override void Dispose()
        {
            this.Clear();
            base.Dispose();
        }
        #endregion
    }
}
