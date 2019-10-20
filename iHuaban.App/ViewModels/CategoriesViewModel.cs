using iHuaban.App.Helpers;
using iHuaban.App.Models;
using iHuaban.Core;
using iHuaban.Core.Commands;
using iHuaban.Core.Controls;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace iHuaban.App.ViewModels
{
    public class CategoriesViewModel : HBPageViewModel
    {

        private List<DataType> CategoryDataTypes { get; set; } = new List<DataType>();

        private IApiHttpHelper HttpHelper { get; set; }
        public CategoriesViewModel(IApiHttpHelper httpHelper, Context context)
            : base(context)
        {
            this.HttpHelper = httpHelper;
            this.CategoryVisibility = Visibility.Collapsed;
            this.CategoryHeaderVisibility = Visibility.Visible;
            this.DataTypes = new ObservableCollection<DataType>()
            {
                new DataType
                {
                    Title ="采集",
                    BaseUrl = "",
                    DataLoaderAsync = LoaderAsync<PinCollection, Pin>,
                    UrlAction = GetCategoryUrl,
                },
                new DataType
                {
                    Title ="画板",
                    BaseUrl = Constants.ApiBoardsName,
                    DataLoaderAsync = LoaderAsync<BoardCollection, Board>,
                    UrlAction = GetCategoryUrl,
                },
                new DataType
                {
                    Title = "用户",
                    BaseUrl =  "users",
                    DataLoaderAsync = LoaderAsync<FavoriteUserCollection, PUser>,
                    UrlAction = GetCategoryUrl,
                    ScaleSize = "4:5",
                },
            };
            this.DataType = DataTypes[0];

            this.Categories = new IncrementalLoadingList<Category>(GetCategoriesAsync);
            this.Categories.Add(Constants.CategoryAll);
            this.Categories.Add(Constants.CategoryHot);
            this.SelectedCategory = Constants.CategoryAll;
            this.Data = new IncrementalLoadingList<IModel>(GetPinsAsync)
            {
                AfterAddItems = o =>
                {
                    if (ExtendedGridView != null)
                    {
                        ExtendedGridView.Width = double.NaN;
                    }
                }
            };

        }

        public override void ViewModelDispose()
        {
            base.ViewModelDispose();
            DataTypes.Clear();
            Categories.Clear();
            Data.Clear();
            Categories = null;
            DataTypes = null;
            Data = null;
        }

        private Visibility _CategoryHeaderVisibility;
        public Visibility CategoryHeaderVisibility
        {
            get { return _CategoryHeaderVisibility; }
            set { SetValue(ref _CategoryHeaderVisibility, value); }
        }

        private Visibility _DataTypesVisibility;
        public Visibility DataTypesVisibility
        {
            get { return _DataTypesVisibility; }
            set { SetValue(ref _DataTypesVisibility, value); }
        }

        private Visibility _CategoryVisibility;
        public Visibility CategoryVisibility
        {
            get { return _CategoryVisibility; }
            set { SetValue(ref _CategoryVisibility, value); }
        }

        public IncrementalLoadingList<IModel> Data { get; set; }

        public ObservableCollection<DataType> DataTypes { private set; get; } = new ObservableCollection<DataType>();

        private DataType _DataType;
        public DataType DataType
        {
            get { return _DataType; }
            set { SetValue(ref _DataType, value); }
        }

        private Category _SelectedCategory;
        public Category SelectedCategory
        {
            get { return _SelectedCategory; }
            set
            {
                SetValue(ref _SelectedCategory, value);
                this.DataTypesVisibility = (value == Constants.CategoryAll || value == Constants.CategoryHot) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private IncrementalLoadingList<Category> _Categories;
        public IncrementalLoadingList<Category> Categories
        {
            get { return _Categories; }
            set
            {
                SetValue(ref _Categories, value);
            }
        }

        private ExtendedGridView ExtendedGridView;
        private DelegateCommand _RefreshCommand;
        public DelegateCommand RefreshCommand
        {
            get
            {
                return _RefreshCommand ?? (_RefreshCommand = new DelegateCommand(
                async o =>
                {
                    try
                    {
                        if (o is ExtendedGridView gridView)
                        {
                            ExtendedGridView = gridView;
                            gridView.Width = (gridView.Parent as Grid).DesiredSize.Width - gridView.Margin.Left - gridView.Margin.Right - 0.6;
                        }
                        await this.Data.ClearAndReload();
                    }
                    catch (Exception)
                    {

                    }

                }, o => true));
            }
        }

        private DelegateCommand _ChangeCategoryCommand;
        public DelegateCommand ChangeCategoryCommand
        {
            get
            {
                return _ChangeCategoryCommand ?? (_ChangeCategoryCommand = new DelegateCommand(
                async o =>
                {
                    try
                    {
                        await this.Data.ClearAndReload();
                    }
                    catch (Exception)
                    {

                    }

                }, o => true));
            }
        }

        private DelegateCommand _GotoTopCommand;
        public DelegateCommand GotoTopCommand
        {
            get
            {
                return _GotoTopCommand ?? (_GotoTopCommand = new DelegateCommand(
                async o =>
                {
                    try
                    {
                        if (o is GridView gridView)
                        {
                            if (gridView?.Items?.Count > 0)
                            {
                                await gridView.ScrollToItem(gridView.Items[0]);
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }

                }, o => true));
            }
        }

        private async Task<IEnumerable<IModel>> LoaderAsync<T, T2>(string url)
            where T : ModelCollection<T2>
            where T2 : IModel
        {
            var result = await this.HttpHelper.GetAsync<T>(url);
            var data = result.Data;
            List<IModel> list = new List<IModel>();
            foreach (var item in data)
            {
                list.Add(item);
            }

            return list;
        }

        private async Task<IEnumerable<IModel>> GetPinsAsync(uint startIndex, int page)
        {
            if (IsLoading || this.DataType == null)
            {
                return new List<IModel>();
            }
            IsLoading = true;
            try
            {
                var url = this.DataType.GetUrl();
                var result = await this.DataType.DataLoaderAsync(url);

                if (result.Count() == 0)
                {
                    NoMoreVisibility = Visibility.Visible;
                    Data.NoMore();
                }
                else
                {
                    NoMoreVisibility = Visibility.Collapsed;
                    Data.HasMore();
                }

                return result;
            }
            catch { }
            finally
            {
                IsLoading = false;
            }
            return null;
        }

        private async Task<IEnumerable<Category>> GetCategoriesAsync(uint startIndex, int page)
        {
            if (IsLoading)
            {
                return new List<Category>();
            }
            IsLoading = true;
            try
            {
                var result = await HttpHelper.GetAsync<CategoryCollection>(Constants.ApiCategories);

                Categories.NoMore();

                return result.Data;
            }
            catch
            {
            }
            finally
            {
                IsLoading = false;
            }
            return null;
        }

        private long GetMaxPinId()
        {
            if (Data?.Count > 0 && long.TryParse(Data?[Data.Count - 1].KeyId, out long maxId))
            {
                return maxId;
            }
            else
            {
                return 0;
            }
        }

        private string GetCategoryUrl(DataType dataType)
        {
            string query = "?limit=20";
            var max = GetMaxPinId();
            if (max > 0)
            {
                query += $"&max={max}";
            }
            return $"{dataType.BaseUrl.Trim('/')}/{SelectedCategory.nav_link.Trim('/')}/{query}";
        }

    }
}
