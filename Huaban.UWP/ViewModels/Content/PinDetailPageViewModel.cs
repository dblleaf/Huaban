using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace Huaban.UWP.ViewModels
{
	using Models;
	using Base;
	using Controls;
	using Commands;

	public class PinDetailPageViewModel : HBViewModel
	{

		public PinDetailPageViewModel(Context context)
			: base(context)
		{
			BoardListViewModel = new BoardListViewModel(context, GetBoardList);
			BoardList = Context.BoardList;
			SelecterVisibility = Visibility.Collapsed;
			CurrentBoardIndex = -1;

		}
		#region Properties

		public BoardListViewModel BoardListViewModel { set; get; }

		public IncrementalLoadingList<Board> BoardList { set; get; }

		private Pin _Pin;
		public Pin Pin
		{
			get { return _Pin; }
			set { SetValue(ref _Pin, value); }
		}

		private string _ImageUri;
		public string ImageUri
		{
			get
			{
				Task.Delay(500);
				return _ImageUri;
			}
			set { SetValue(ref _ImageUri, value); }
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
						else if (type.IndexOf("bmp") >= 0)
							type = "bmp";
						else if (type.IndexOf("gif") >= 0)
							type = "gif";
						else
							type = "jpg";

						await StorageHelper.SaveImage(buffer, $"{DateTime.Now.Ticks}.{type}", "huaban");
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

		//图片加载完毕
		private DelegateCommand _LoadedCommand;
		public DelegateCommand LoadedCommand
		{
			get
			{
				AppBarButton aaa = new AppBarButton();

				return _LoadedCommand ?? (_LoadedCommand = new DelegateCommand(
					o =>
					{
						IsLoading = false;
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
					var board = o as Board;
					if (board == null)
						return;
					var pin = await Context.API.PinAPI.Pin(Pin.pin_id, board.board_id, Pin.raw_text);
					if (board.cover == null)
						board.cover = pin;
					Context.ShowTip("采集成功");
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
					var board = await Context.API.BoardAPI.add(o.ToString());
					if (board != null)
					{
						var list = Context.BoardList;
						list.Add(board);
						var pin = await Context.API.PinAPI.Pin(Pin.pin_id, board.board_id, Pin.raw_text);
						board.pins.Add(pin);
						board.cover = pin;
						Context.ShowTip("采集成功");
					}
				}, o => true));
			}
		}

		#endregion

		#region Methods

		private async Task<IEnumerable<Board>> GetBoardList(uint startIndex, int page)
		{
			BoardListViewModel.BoardList.NoMore();
			IsLoading = true;

			List<Board> list = new List<Board>();

			try
			{
				list = await Context.API.PinAPI.GetRelatedBoards(Pin?.pin_id, BoardListViewModel.GetMaxSeq());

				if (list.Count == 0)
					BoardListViewModel.BoardList.NoMore();
				else
					BoardListViewModel.BoardList.HasMore();
			}
			catch (Exception ex)
			{
				string a = ex.Message;
			}
			finally
			{
				IsLoading = false;
			}
			return list;
		}

		public async override void OnNavigatedTo(HBNavigationEventArgs e)
		{
			var pin = e.Parameter as Pin;

			if (e.NavigationMode == NavigationMode.New)
			{
				IsLoading = true;

				if (pin?.pin_id != Pin?.pin_id)
				{
					Pin = await Context.API.PinAPI.GetPin(pin.pin_id);
				}

				PivotSelectedIndex = 0;
				ImageUri = Pin.file.Orignal;
				//Pin = await App.API.PinAPI.GetPin(pin.pin_id);
				Liked = Pin.liked;
				await BoardListViewModel.ClearAndReload();
			}
		}

		private void NavigationService_BackEvent(object sender, BackRequestedEventArgs e)
		{
			if (!e.Handled)
			{
				e.Handled = true;
				SelecterVisibility = Visibility.Collapsed;
			}
		}
		#endregion
	}
}
