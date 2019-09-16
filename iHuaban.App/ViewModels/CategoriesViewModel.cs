using iHuaban.App.Helpers;
using iHuaban.App.Models;
using iHuaban.App.TemplateSelectors;
using iHuaban.Core;
using iHuaban.Core.Commands;
using iHuaban.Core.Controls;
using iHuaban.Core.Helpers;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using static System.Net.WebUtility;

namespace iHuaban.App.ViewModels
{
    public class CategoriesViewModel : PageViewModel
    {
        private List<DataType> CategoryDataTypes { get; set; } = new List<DataType>();
        private List<DataType> SearchDataTypes { get; set; } = new List<DataType>();

        private IApiHttpHelper HttpHelper { get; set; }
        public IValueConverter ValueConverter { get; set; }
        public CategoriesViewModel(IApiHttpHelper httpHelper, IValueConverter valueConverter)
        {
            this.HttpHelper = httpHelper;
            this.CategoryVisibility = Visibility.Collapsed;
            this.CategoryHeaderVisibility = Visibility.Visible;
            this.ValueConverter = valueConverter;
            this.CategoryDataTypes = new List<DataType>()
            {
                new DataType
                {
                    Type ="采集",
                    BaseUrl = "",
                    DataLoaderAsync = LoaderAsync<PinCollection, Pin>,
                    UrlAction = GetCategoryUrl,
                },
                new DataType
                {
                    Type ="画板",
                    BaseUrl = Constants.ApiBoardsName,
                    DataLoaderAsync = LoaderAsync<BoardCollection, Board>,
                    UrlAction = GetCategoryUrl,
                },
                new DataType
                {
                    Type = "用户",
                    BaseUrl =  "users",
                    DataLoaderAsync = LoaderAsync<FavoriteUserCollection, PUser>,
                    UrlAction = GetCategoryUrl,
                    ScaleSize = "4:5",
                },
            };

            ChangeDataTypes(this.CategoryDataTypes);

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

        public override string Icon => Constants.IconFind;
        public override string Title => Constants.TextFind;
        public override string TemplateName => Constants.TemplateFind;
        public string ScaleSize => "300:300";
        public decimal CellMinWidth => 236;
        public DataTemplateSelector DataTemplateSelector { get; private set; } = new SupperDataTemplateSelector();

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

        private IncrementalLoadingList<IModel> _Data;
        public IncrementalLoadingList<IModel> Data
        {
            get { return _Data; }
            set { SetValue(ref _Data, value); }
        }

        public ObservableCollection<DataType> DataTypes { get; } = new ObservableCollection<DataType>();

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

        private string _SearchText;
        public string SearchText
        {
            get { return _SearchText; }
            set { SetValue(ref _SearchText, value); }
        }

        private string _SearchKey;
        public string SearchKey
        {
            get { return _SearchKey; }
            set { SetValue(ref _SearchKey, value); }
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

        private DelegateCommand _SetCategoryVisibilityCommand;
        public DelegateCommand SetCategoryVisibilityCommand
        {
            get
            {
                return _SetCategoryVisibilityCommand ?? (_SetCategoryVisibilityCommand = new DelegateCommand(
                o =>
                {
                    try
                    {
                        this.CategoryVisibility = o.ToString().Equals("true", StringComparison.CurrentCultureIgnoreCase) ? Visibility.Visible : Visibility.Collapsed;
                    }
                    catch (Exception)
                    {

                    }

                }, o => true));
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

                        this.ChangeDataTypes(this.CategoryDataTypes);

                        this.CategoryHeaderVisibility = Visibility.Visible;
                        this.SearchKey = string.Empty;
                        this.SearchText = string.Empty;

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

        private int currentPage = 0;
        private string GetSearchUrl(DataType dataType)
        {
            string url = $"{dataType.BaseUrl}?q={UrlEncode(this.SearchKey)}&page={++currentPage}&per_page=20";
            return url;
        }

        private void ChangeDataTypes(IEnumerable<DataType> dataTypes)
        {
            this.DataTypes.Clear();
            if (dataTypes?.Count() > 0)
            {
                foreach (var dataType in dataTypes)
                {
                    this.DataTypes.Add(dataType);
                }
                this.DataType = DataTypes[0];
            }
        }
    }
}
