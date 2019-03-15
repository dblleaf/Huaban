using iHuaban.App.Models;
using iHuaban.App.TemplateSelectors;
using iHuaban.Core;
using iHuaban.Core.Commands;
using iHuaban.Core.Helpers;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace iHuaban.App.ViewModels
{
    public class CategoryViewModel : ViewModelBase
    {
        private IHttpHelper HttpHelper { get; set; }
        public CategoryViewModel(IHttpHelper httpHelper)
        {
            this.HttpHelper = httpHelper;
            this.CategoryVisibility = Visibility.Collapsed;
            this.GridViewHorizontalAlignment = HorizontalAlignment.Stretch;
            this.DataTypes = new ObservableCollection<DataType>()
            {
                new DataType
                {
                    Type ="采集",
                    Url = Constants.ApiBase,
                    DataLoaderAsync =LoaderAsync<PinCollection, Pin>,
                },
                new DataType
                {
                    Type ="推荐画板",
                    Url = Constants.ApiBoards,
                    DataLoaderAsync = LoaderAsync<BoardCollection, Board>,
                },
                new DataType
                {
                    Type = "推荐用户",
                    Url =  Constants.ApiUsers,
                    DataLoaderAsync =LoaderAsync<FavoriteUserCollection, PUser>,
                    ScaleSize = "4:5",
                },
            };

            this.Categories = new IncrementalLoadingList<Category>(GetCategoriesAsync);
            this.Categories.Add(Constants.CategoryAll);
            this.Categories.Add(Constants.CategoryHot);
            this.SelectedCategory = Constants.CategoryAll;
            this.Pins = new IncrementalLoadingList<IModel>(GetPinsAsync);

            this.Pins.AfterAddItems += o =>
            {
                this.GridViewHorizontalAlignment = HorizontalAlignment.Stretch;
            };

            this.DataType = DataTypes[0];
        }

        public override string Icon => Constants.IconCategory;
        public override string Title => Constants.TextCategory;
        public override string TemplateName => Constants.TemplateCategories;
        public string ScaleSize => "300:300";
        public decimal CellMinWidth => 236;
        public DataTemplateSelector DataTemplateSelector { get; private set; } = new SupperDataTemplateSelector();

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

        private IncrementalLoadingList<IModel> _Pins;
        public IncrementalLoadingList<IModel> Pins
        {
            get { return _Pins; }
            set { SetValue(ref _Pins, value); }
        }

        public ObservableCollection<DataType> DataTypes { get; }

        private DataType _DataType;
        public DataType DataType
        {
            get { return _DataType; }
            set { SetValue(ref _DataType, value); }
        }

        private HorizontalAlignment _GridViewHorizontalAlignment;
        public HorizontalAlignment GridViewHorizontalAlignment
        {
            get { return _GridViewHorizontalAlignment; }
            set { SetValue(ref _GridViewHorizontalAlignment, value); }
        }

        private Category _SelectedCategory;
        public Category SelectedCategory
        {
            get { return _SelectedCategory; }
            set
            {
                DataTypesVisibility = (value == Constants.CategoryAll || value == Constants.CategoryHot) ? Visibility.Collapsed : Visibility.Visible;
                if (DataTypesVisibility == Visibility.Collapsed)
                {
                    this.DataType = this.DataTypes[0];
                }
                SetValue(ref _SelectedCategory, value);
            }
        }

        private IncrementalLoadingList<Category> _Categories;
        public IncrementalLoadingList<Category> Categories
        {
            get { return _Categories; }
            set { SetValue(ref _Categories, value); }
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
                    catch (Exception ex)
                    {

                    }

                }, o => true));
            }
        }

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
                        this.GridViewHorizontalAlignment = HorizontalAlignment.Left;
                        await this.Pins.ClearAndReload();
                    }
                    catch (Exception ex)
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
            if (IsLoading)
            {
                return new List<IModel>();
            }
            IsLoading = true;
            try
            {
                bool isUsers = this.DataType == this.DataTypes[2];
                if (isUsers)
                {

                }
                var result = await this.DataType.DataLoaderAsync(GetUrl());

                if (result.Count() == 0)
                {
                    NoMoreVisibility = Visibility.Visible;
                    Pins.NoMore();
                }
                else
                {
                    NoMoreVisibility = Visibility.Collapsed;
                    Pins.HasMore();
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
            if (Pins?.Count > 0 && long.TryParse(Pins?[Pins.Count - 1].KeyId, out long maxId))
            {
                return maxId;
            }
            else
            {
                return 0;
            }
        }

        private string GetUrl()
        {
            string query = "?limit=20";
            var max = GetMaxPinId();
            if (max > 0)
            {
                query += $"&max={max}";
            }
            return $"{this.DataType.Url.Trim('/')}/{SelectedCategory.nav_link.Trim('/')}/{query}";
        }

    }
}