using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.UI.Popups;

namespace Huaban.UWP.ViewModels
{
	using Models;
	using Controls;
	using Base;
	using Commands;

	public class BoardDetailViewModel : HBViewModel
	{
		public BoardDetailViewModel(Context context)
			: base(context)
		{
			LeftHeaderVisibility = Visibility.Collapsed;
			Title = "编辑画板";
			CategoryList = Context.Categories;

		}

		#region Properties
		public ObservableCollection<Category> CategoryList { get; private set; }
		private Board _Board;
		public Board Board
		{
			get { return _Board; }
			set
			{ SetValue(ref _Board, value); }
		}

		private Category _CurrentCategory;
		public Category CurrentCategory
		{
			get { return _CurrentCategory; }
			set { SetValue(ref _CurrentCategory, value); }
		}

		#endregion

		#region Commands

		private DelegateCommand _DeleteBoardCommand;
		public DelegateCommand DeleteBoardCommand
		{
			get
			{
				return _DeleteBoardCommand ?? (_DeleteBoardCommand = new DelegateCommand(
				async o =>
				{

					var dialog = new MessageDialog("确定要删除这个画板吗？删除后这个画板内的所有采集都会被删除。", "提示");
					UICommand yes = new UICommand("删除", async c =>
					{
						await Context.API.BoardAPI.delete(Board);//远程服务器删除
						Context.ShowTip("删除成功！");

						Context.BoardListVM.BoardList.Remove(Board);//从自己的画板列表中删除
						Context.NavigationService.GoBack();
						
					});
					UICommand no = new UICommand("取消");
					dialog.Commands.Add(yes);
					dialog.Commands.Add(no);

					await dialog.ShowAsync();

				}, o => true));
			}
		}

		private DelegateCommand _UpdateBoardCommand;
		public DelegateCommand UpdateBoardCommand
		{
			get
			{
				return _UpdateBoardCommand ?? (_UpdateBoardCommand = new DelegateCommand(
				async o =>
				{
					Board.category_id = CurrentCategory?.id;

					await Context.API.BoardAPI.edit(Board);

					Context.ShowTip("编辑成功！");
				}, o => true));
			}
		}
		#endregion

		#region Methods

		public async override void OnNavigatedTo(HBNavigationEventArgs e)
		{
			IsLoading = true;

			try
			{
				var board = e.Parameter as Board;
				if (board == null || board == Board)
					return;

				await Task.Delay(300);

				CurrentCategory = CategoryList.FirstOrDefault(o => o.id == board.category_id);
				Board = board;
			}
			catch (Exception ex)
			{ }
			finally
			{
				IsLoading = false;
			}
		}

		public override Size ArrangeOverride(Size finalSize)
		{
			return finalSize;
		}
		#endregion

	}
}
