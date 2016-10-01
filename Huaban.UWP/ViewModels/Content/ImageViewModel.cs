using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Input;
using Windows.Storage;
using Windows.ApplicationModel.DataTransfer;
namespace Huaban.UWP.ViewModels
{
	using Base;
	using Controls;
	using Models;
	using Commands;

	public class ImageViewModel : HBViewModel
	{
		public ImageViewModel(Context context)
			: base(context)
		{
			BoardList = Context.BoardListVM?.BoardList;
			SelecterVisibility = Visibility.Collapsed;
			CurrentBoardIndex = -1;

			QuickBoardChanged += (s, e) =>
			{
				InitQuickBoard();
			};
			InitQuickBoard();
		}

		#region Properties
		public IncrementalLoadingList<Board> BoardList { set; get; }

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

		private bool _ImageLoaded;
		public bool ImageLoaded
		{
			get { return _ImageLoaded; }
			set { SetValue(ref _ImageLoaded, value); }
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
					this.Context.NavigationService.BackEvent += NavigationService_BackEvent;
				else
					this.Context.NavigationService.BackEvent -= NavigationService_BackEvent;
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
						var img = await ImageLib.ImageLoader.Instance.LoadImageStream(new Uri(Pin.file.Orignal), new CancellationTokenSource(TimeSpan.FromMilliseconds(1000 * 10)));

						await StorageHelper.SaveAsync($"{DateTime.Now.Ticks}.{type}", img, "huaban");
						Context.ShowTip("下载成功");

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
						string str = await Context.API.PinAPI.Like(Pin.pin_id, !Pin.liked);

						Liked = (str != "{}");

						Context.ShowTip(Liked ? "已设置为喜欢" : "已取消喜欢");
					}, o => true)
				);
			}
		}

		//返回主界面
		private DelegateCommand _HideCommand;
		public DelegateCommand HideCommand
		{
			get
			{
				return _HideCommand ?? (_HideCommand = new DelegateCommand(
					obj =>
					{
						Context.NavigationService.GoBack();
					}, o => true)
				);
			}
		}

		private DelegateCommand _LoadingStartedCommand;
		public DelegateCommand LoadingStartedCommand
		{
			get
			{
				return _LoadingStartedCommand ?? (_LoadingStartedCommand = new DelegateCommand(
					o =>
					{
						IsLoading = true;
						ImageLoaded = true;
					}, o => true)
				);
			}
		}
		//图片加载完毕
		private DelegateCommand _LoadedCommand;
		public DelegateCommand LoadedCommand
		{
			get
			{
				return _LoadedCommand ?? (_LoadedCommand = new DelegateCommand(
					o =>
					{
						IsLoading = false;
						ImageLoaded = true;
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
					var args = o as ItemClickEventArgs;
					var item = o as Board;
					if (args == null && item == null)
						return;

					if (args != null)
						item = args.ClickedItem as Board;

					var pin = await Context.API.PinAPI.Pin(Pin.pin_id, item.board_id, Pin.raw_text);
					if (item.cover == null)
						item.cover = pin;
					Context.ShowTip($"采集到了画板：{item.title}");
					QuickBoard = item;
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
					if (string.IsNullOrEmpty(NewBoardName))
						return;
					string boardName = NewBoardName;
					NewBoardName = "";
					var board = await Context.API.BoardAPI.add(boardName);

					if (board != null)
					{
						var list = Context.BoardListVM.BoardList;
						list.Add(board);
						var pin = await Context.API.PinAPI.Pin(Pin.pin_id, board.board_id, Pin.raw_text);
						board.pins.Add(pin);
						board.cover = pin;
						Context.ShowTip($"采集到了画板：{board.title}");
						QuickBoard = board;
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
					var e = o as KeyRoutedEventArgs;

					if (e?.Key == Windows.System.VirtualKey.Enter)
					{
						var txt = e.OriginalSource as TextBox;
						NewBoardName = txt.Text;
						NewBoardCommand.Execute(e.OriginalSource);
						SelecterVisibility = Visibility.Collapsed;
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
					dp.SetText($"http://huaban.com/pins/{Pin?.pin_id}");
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
					await Windows.System.Launcher.LaunchUriAsync(new Uri($"http://huaban.com/pins/{Pin?.pin_id}"));
				}, o => true));
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
			PinListViewModel = new PinListViewModel(Context, null);

			base.Dispose();
		}

		#endregion
	}
}
