using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Huaban.UWP.ViewModels
{
    using Base;
    using Commands;
    using Controls;
    using Models;
    using Services;
    using Unity.Attributes;

    public class ImageViewModel : HBViewModel
    {
        public ImageViewModel(Context context)
            : base(context)
        {
            SelecterVisibility = Visibility.Collapsed;
            CurrentBoardIndex = -1;
            RawTextVisibility = Visibility.Visible;
            ButtonChar = '';

            //double marginTop = 0;
            //if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            //{
            //    marginTop = -32;

            //    StatusBar.GetForCurrentView().ForegroundColor = Setting.Current.DarkMode ? Colors.White : Colors.Black;
            //}
            //Margin = new Thickness(0, marginTop, 0, 0);

            QuickBoardChanged += (s, e) =>
            {
                InitQuickBoard();
            };
            InitQuickBoard();
        }

        #region Properties
        [Dependency]
        public PinService PinService { get; set; }
        [Dependency]
        public BoardService BoardService { get; set; }

        private Thickness _Margin;
        public Thickness Margin
        {
            get { return _Margin; }
            set { SetValue(ref _Margin, value); }
        }

        public IncrementalLoadingList<Board> BoardList { get { return Context.BoardListVM?.BoardList; } }

        private PinListViewModel _PinListViewModel;
        public PinListViewModel PinListViewModel
        {
            get { return _PinListViewModel; }
            set { SetValue(ref _PinListViewModel, value); }
        }

        public Pin Pin
        {
            get { return PinListViewModel.SelectedItem; }
        }

        private IconElement _Icon = new FontIcon() { Glyph = "" };
        public IconElement Icon
        {
            get { return _Icon; }
            set { SetValue(ref _Icon, value); }
        }

        public bool Liked
        {
            set
            {
                Pin.liked = value;
                if (value)
                    Icon = new FontIcon() { Glyph = "", Foreground = new SolidColorBrush(Colors.Red) };
                else
                    Icon = new FontIcon() { Glyph = "" };
            }
            get { return Pin.liked; }
        }

        private Visibility _SelecterVisibility;
        public Visibility SelecterVisibility
        {
            get { return _SelecterVisibility; }
            set
            {
                SetValue(ref _SelecterVisibility, value);
                if (SelecterVisibility == Visibility.Visible)
                    NavigationService.BackEvent += NavigationService_BackEvent;
                else
                    NavigationService.BackEvent -= NavigationService_BackEvent;
            }
        }

        private Board _CurrentBoard;
        public Board CurrentBoard
        {
            get { return _CurrentBoard; }
            set { SetValue(ref _CurrentBoard, value); }
        }

        private int _CurrentBoardIndex;
        public int CurrentBoardIndex
        {
            get { return _CurrentBoardIndex; }
            set { SetValue(ref _CurrentBoardIndex, value); }
        }

        private int _PivotSelectedIndex;
        public int PivotSelectedIndex
        {
            get { return _PivotSelectedIndex; }
            set { SetValue(ref _PivotSelectedIndex, value); }
        }

        private string _NewBoardName;
        public string NewBoardName
        {
            get { return _NewBoardName; }
            set { SetValue(ref _NewBoardName, value); }
        }

        private Visibility _RawTextVisibility;
        public Visibility RawTextVisibility
        {
            get { return _RawTextVisibility; }
            set { SetValue(ref _RawTextVisibility, value); }
        }
        private char _ButtonChar;
        public char ButtonChar
        {
            get { return _ButtonChar; }
            set { SetValue(ref _ButtonChar, value); }
        }
        #endregion

        #region Commands

        //下载
        private DelegateCommand _DownloadCommand;
        public DelegateCommand DownLoadCommand
        {
            get
            {
                AppBarButton aaa = new AppBarButton();

                return _DownloadCommand ?? (_DownloadCommand = new DelegateCommand(
                    async o =>
                    {
                        if (Pin == null)
                            return;
                        var buffer = await HttpHelper.Factory.GetBytes(Pin.file.Orignal);
                        string type = Pin.file.type?.ToLower();
                        if (type?.IndexOf("png") >= 0)
                            type = "png";
                        else if (type?.IndexOf("bmp") >= 0)
                            type = "bmp";
                        else if (type?.IndexOf("gif") >= 0)
                            type = "gif";
                        else
                            type = "jpg";
                        try
                        {
                            //var img = await ImageLib.ImageLoader.Instance.LoadImageStream(new Uri(Pin.file.Orignal), new CancellationTokenSource(TimeSpan.FromMilliseconds(1000 * 10)));

                            await StorageHelper.SaveImage(buffer, $"{DateTime.Now.Ticks}.{type}");

                            Context.ShowTip("下载成功");
                        }
                        catch (Exception ex)
                        {
                            Context.ShowTip("发生异常，请重新尝试此操作！");
                        }

                    }, o => true)
                );
            }
        }

        //喜欢/取消喜欢
        private DelegateCommand _LikeCommand;
        public DelegateCommand LikeCommand
        {
            get
            {
                return _LikeCommand ?? (_LikeCommand = new DelegateCommand(
                    async o =>
                    {
                        try
                        {
                            string str = await PinService.Like(Pin.pin_id, !Pin.liked);

                            Liked = (str != "{}");

                            Context.ShowTip(Liked ? "已设置为喜欢" : "已取消喜欢");
                        }
                        catch (Exception ex)
                        {

                        }

                    }, o => true)
                );
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
                    SelecterVisibility = Visibility.Visible;
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
                    try
                    {
                        var args = o as ItemClickEventArgs;
                        var item = o as Board;
                        if (args == null && item == null)
                            return;

                        if (args != null)
                            item = args.ClickedItem as Board;

                        var pin = await PinService.Pin(Pin.pin_id, item.board_id, Pin.raw_text);
                        if (item.cover == null)
                            item.cover = pin;
                        Context.ShowTip($"采集到了画板：{item.title}");
                        QuickBoard = item;
                    }
                    catch (Exception ex)
                    {

                    }

                }, o => true));
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
                    try
                    {
                        if (string.IsNullOrEmpty(NewBoardName))
                            return;
                        string boardName = NewBoardName;
                        NewBoardName = "";
                        var board = await BoardService.add(boardName);

                        if (board != null)
                        {
                            var list = Context.BoardListVM.BoardList;
                            list.Add(board);
                            var pin = await PinService.Pin(Pin.pin_id, board.board_id, Pin.raw_text);
                            board.pins.Add(pin);
                            board.cover = pin;
                            Context.ShowTip($"采集到了画板：{board.title}");
                            QuickBoard = board;
                        }
                    }
                    catch (Exception ex)
                    {

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
                    try
                    {
                        var e = o as KeyRoutedEventArgs;

                        if (e?.Key == Windows.System.VirtualKey.Enter)
                        {
                            var txt = e.OriginalSource as TextBox;
                            NewBoardName = txt.Text;
                            NewBoardCommand.Execute(e.OriginalSource);
                            SelecterVisibility = Visibility.Collapsed;
                        }
                    }
                    catch (Exception ex)
                    {

                    }

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
                    SelectBoardCommand.Execute(QuickBoard);
                }, o => true));
            }
        }

        //复制地址到剪贴板
        private DelegateCommand _CopyLinkCommmand;
        public DelegateCommand CopyLinkCommmand
        {
            get
            {
                return _CopyLinkCommmand ?? (_CopyLinkCommmand = new DelegateCommand(
                o =>
                {
                    DataPackage dp = new DataPackage();
                    dp.SetText($"http://huabanpro.com/pins/{Pin?.pin_id}");
                    Clipboard.SetContent(dp);

                    Context.ShowTip("地址已复制到剪贴板！");
                }, o => true));
            }
        }

        //在浏览器中打开
        private DelegateCommand _OpenInBrowser;
        public DelegateCommand OpenInBrowser
        {
            get
            {
                return _OpenInBrowser ?? (_OpenInBrowser = new DelegateCommand(
                async o =>
                {
                    await Windows.System.Launcher.LaunchUriAsync(new Uri($"http://huabanpro.com/pins/{Pin?.pin_id}"));
                }, o => true));
            }
        }

        private DelegateCommand _ToggleRowTextCommand;
        public DelegateCommand ToggleRowTextCommand
        {
            get
            {
                return _ToggleRowTextCommand ?? (_ToggleRowTextCommand = new DelegateCommand(
                    o =>
                    {
                        try
                        {
                            var ooo = o as TappedRoutedEventArgs;
                            if (ooo != null)
                                ooo.Handled = true;

                            if (RawTextVisibility == Visibility.Visible)
                            {
                                RawTextVisibility = Visibility.Collapsed;
                                ButtonChar = '';//E010
                            }
                            else
                            {
                                RawTextVisibility = Visibility.Visible;
                                ButtonChar = '';//E011
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                    }, o => true)
                );
            }
        }
        #endregion

        #region Methods

        public override void OnNavigatedTo(HBNavigationEventArgs e)
        {
            PinListViewModel model = e.Parameter as PinListViewModel;
            if (model != null)
            {
                PinListViewModel = model;
                PinListViewModel.OnSelectedChanged += PinListViewModel_OnSelectedChanged;
            }
        }

        private void PinListViewModel_OnSelectedChanged(Pin args)
        {
            if (Pin != null)
                Liked = Pin.liked;
        }

        public override bool OnNavigatingFrom(HBNavigatingCancelEventArgs e)
        {

            if (PinListViewModel != null)
                PinListViewModel.OnSelectedChanged -= PinListViewModel_OnSelectedChanged;
            return base.OnNavigatingFrom(e);
        }
        private void NavigationService_BackEvent(object sender, BackRequestedEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                SelecterVisibility = Visibility.Collapsed;
            }
        }

        public override void Dispose()
        {
            PinListViewModel = null;

            base.Dispose();
        }
        #endregion
    }
}
