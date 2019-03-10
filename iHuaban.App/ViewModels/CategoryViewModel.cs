using iHuaban.App.Models;
using iHuaban.Core.Commands;
using iHuaban.Core.Helpers;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace iHuaban.App.ViewModels
{
    public class CategoryViewModel : ViewModelBase
    {
        private IHttpHelper HttpHelper { get; set; }
        public CategoryViewModel(IHttpHelper httpHelper)
        {
            this.HttpHelper = httpHelper;
            this.CategoryVisibility = Visibility.Collapsed;
            this.DataTypes = new ObservableCollection<DataType>()
            {
                new DataType("采集", Constants.ApiBase, LoaderAsync<PinCollection ,Pin>),
                new DataType("画板", Constants.ApiBoards, LoaderAsync<BoardCollection, Board>),
                new DataType("用户", Constants.ApiUsers, LoaderAsync<UserCollection, User>),
            };

            this.Categories = new IncrementalLoadingList<Category>(GetCategoriesAsync);
            this.Categories.Add(Constants.CategoryAll);
            this.Categories.Add(Constants.CategoryHot);
            this.SelectedCategory = Constants.CategoryAll;
            this.Pins = new IncrementalLoadingList<IModel>(GetPinsAsync);

            this.DataType = DataTypes[0];
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

        private Category _SelectedCategory;
        public Category SelectedCategory
        {
            get { return _SelectedCategory; }
            set { SetValue(ref _SelectedCategory, value); }
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
                        await this.Pins.ClearAndReload();
                    }
                    catch (Exception ex)
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
                var result = await this.DataType.DataLoaderAsync(GetUrl());

                if (result.Count() == 0)
                {
                    NoMoreVisibility = Visibility.Visible;
                    Pins.NoMore();
                }
                else
                    Pins.HasMore();
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