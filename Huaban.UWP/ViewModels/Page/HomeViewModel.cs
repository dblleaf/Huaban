using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Core;

namespace Huaban.UWP.ViewModels
{
	using Base;
	using Services;
	using Models;
	using Commands;
	using Windows.Foundation;
	using Controls;

	public class HomeViewModel : HBViewModel
	{
		public HomeViewModel(Context context)
			: base(context)
		{
			Title = "发现";
			PinListViewModel = new PinListViewModel(context, GetData);
			PinListViewModel.TargetName = "HomePage";
			CategoryList = Context.CategoryList;
			SelecterVisibility = Visibility.Collapsed;

			QuickBoardChanged += (s, e) =>
			{
				QuickBoardName = QuickBoard?.title;
				CanQuick = QuickBoard != null;
			};
		}

		#region Properties

		private Category _CurrentCategory;
		public Category CurrentCategory
		{
			get { return _CurrentCategory; }
			set
			{
				SetValue(ref _CurrentCategory, value);
				Title = value?.name;
			}
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

		public PinListViewModel PinListViewModel { set; get; }
		public IncrementalLoadingList<Category> CategoryList { get; private set; }

		private bool _ShowSearchBox;
		public bool ShowSearchBox
		{
			get { return _ShowSearchBox; }
			set { SetValue(ref _ShowSearchBox, value); }
		}

		#endregion

		#region Commands

		private DelegateCommand _ChangeCategoryCommand;
		public DelegateCommand ChangeCategoryCommand
		{
			get
			{
				return _ChangeCategoryCommand ?? (_ChangeCategoryCommand = new DelegateCommand(
					async (Object obj) =>
					{
						await PinListViewModel.ClearAndReload();
					},
					(Object obj) => true)
				);
			}
		}

		private DelegateCommand _ShowSelectCommand;
		public DelegateCommand ShowSelectCommand
		{
			get
			{
				return _ShowSelectCommand ?? (_ShowSelectCommand = new DelegateCommand(
				o =>
				{
					SelecterVisibility = Visibility.Visible;
				}, o => true));
			}
		}

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

		private DelegateCommand _ToSearchCommand;
		public DelegateCommand ToSearchCommand
		{
			get
			{
				return _ToSearchCommand ?? (_ToSearchCommand = new DelegateCommand(
				o =>
				{
					this.Context.NavigationService.NavigateTo("Search", null, "Search");
				}, o => true));
			}
		}

		#endregion

		#region Methods

		public override Size ArrangeOverride(Size finalSize)
		{
			base.ArrangeOverride(finalSize);
			PinListViewModel?.SetWidth(finalSize.Width);
			return finalSize;
		}
		public async override void Inited()
		{
			base.Inited();

			IsLoading = true;


			try
			{
				await CategoryList.LoadMoreItemsAsync(0);
				CurrentCategory = CategoryList[0];
				await PinListViewModel.ClearAndReload();
			}
			catch (Exception ex)
			{ }
			finally
			{
				IsLoading = false;
			}

		}

		private async Task<IEnumerable<Pin>> GetData(uint startIndex, int page)
		{
			if (CurrentCategory == null)
				return new List<Pin>();

			IsLoading = true;
			try
			{
				var list = await Context.API.CategoryAPI.GetCategoryPinList(CurrentCategory.nav_link, 20, PinListViewModel.GetMaxPinID());
				foreach (var item in list)
				{
					item.Width = PinListViewModel.ColumnWidth;
					if (item.file != null)
						item.Height = ((PinListViewModel.ColumnWidth - 0.8) * item.file.height / item.file.width);
				}
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
