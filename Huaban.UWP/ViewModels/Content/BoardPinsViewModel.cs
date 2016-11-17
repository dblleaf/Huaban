using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.ApplicationModel.DataTransfer;

namespace Huaban.UWP.ViewModels
{
	using Models;
	using Controls;
	using Base;
	using Commands;

	public class BoardPinsViewModel : HBViewModel
	{
		public BoardPinsViewModel(Context context)
			: base(context)
		{
			PinListViewModel = new PinListViewModel(context, GetData);
			Title = "画板";
			LeftHeaderVisibility = Visibility.Collapsed;
		}

		#region Properties

		public PinListViewModel PinListViewModel { set; get; }

		private Board _CurrentBoard;
		public Board CurrentBoard
		{
			get { return _CurrentBoard; }
			set
			{
				SetValue(ref _CurrentBoard, value);
				Title = _CurrentBoard.title;

				SetVisibility();
			}
		}

		private Visibility _FollowVisibility;
		public Visibility FollowVisibility
		{
			get { return _FollowVisibility; }
			set { SetValue(ref _FollowVisibility, value); }
		}

		private Visibility _UnFollowVisibility;
		public Visibility UnFollowVisibility
		{
			get { return _UnFollowVisibility; }
			set { SetValue(ref _UnFollowVisibility, value); }
		}
		#endregion

		#region Commands

		private DelegateCommand _FollowBoardCommand;
		public DelegateCommand FollowBoardCommand
		{
			get
			{
				return _FollowBoardCommand ?? (_FollowBoardCommand = new DelegateCommand(
				async o =>
				{
					string str = await Context.API.BoardAPI.follow(CurrentBoard.board_id, !CurrentBoard.following);

					CurrentBoard.following = (str != "{}");

					SetVisibility();
					Context.ShowTip(CurrentBoard.following ? "关注成功" : "已取消关注");
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
					dp.SetText($"http://huaban.com/boards/{CurrentBoard.board_id}");
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
					await Windows.System.Launcher.LaunchUriAsync(new Uri($"http://huaban.com/boards/{CurrentBoard.board_id}"));
				}, o => true));
			}
		}
		#endregion

		#region Methods

		private async Task<IEnumerable<Pin>> GetData(uint startIndex, int page)
		{
			if (CurrentBoard == null)
				return new List<Pin>();

			IsLoading = true;
			try
			{
				var list = await Context.API.BoardAPI.GetPins(CurrentBoard.board_id, PinListViewModel.GetMaxPinID());
				foreach (var item in list)
				{
					item.Width = PinListViewModel.ColumnWidth;
					if (item.file != null)
						item.Height = ((PinListViewModel.ColumnWidth - 0.8) * item.file.height / item.file.width);
				}
				if (list.Count == 0)
					PinListViewModel.PinList.NoMore();
				else
					PinListViewModel.PinList.HasMore();
				return list;
			}
			catch (Exception ex)
			{
			}
			finally
			{
				IsLoading = false;
			}
			return null;
		}

		public async override void OnNavigatedTo(HBNavigationEventArgs e)
		{
			try
			{
				var board = e.Parameter as Board;
				if (board == null || board == CurrentBoard)
					return;

				CurrentBoard = await Context.API.BoardAPI.GetBoard(board.board_id);
				await PinListViewModel.ClearAndReload();
			}
			catch (Exception ex)
			{ }
		}
		
		private void SetVisibility()
		{
			if (!IsLogin || CurrentBoard?.user_id == Context?.User?.user_id)
				FollowVisibility = UnFollowVisibility = Visibility.Collapsed;
			else {
				UnFollowVisibility = CurrentBoard.following ? Visibility.Visible : Visibility.Collapsed;
				FollowVisibility = CurrentBoard.following ? Visibility.Collapsed : Visibility.Visible;
			}
		}

		public override Size ArrangeOverride(Size finalSize)
		{
			return finalSize;
		}

		public override void Dispose()
		{
			
			PinListViewModel.Dispose();
			base.Dispose();
		}
		#endregion


	}
}
