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
    public class FindViewModel : PageViewModel
    {
        private IApiHttpHelper HttpHelper { get; set; }
        public IValueConverter ValueConverter { get; set; }
        public FindViewModel(IApiHttpHelper httpHelper, IValueConverter valueConverter)
        {
            this.HttpHelper = httpHelper;
            this.ValueConverter = valueConverter;

            this.DataTypes = new ObservableCollection<DataType>()
            {
                new DataType
                {
                    Type ="采集",
                    BaseUrl = "search",
                    DataLoaderAsync =LoaderAsync<PinCollection, Pin>,
                    UrlAction = GetSearchUrl,
                },
                new DataType
                {
                    Type ="画板",
                    BaseUrl = "search/boards",
                    DataLoaderAsync = LoaderAsync<BoardCollection, Board>,
                    UrlAction = GetSearchUrl,
                },
                new DataType
                {
                    Type = "用户",
                    BaseUrl =  "search/users",
                    DataLoaderAsync = LoaderAsync<UserCollection, User>,
                    UrlAction = GetSearchUrl,
                    ScaleSize = "4:5",
                },
            };

            this.DataType = this.DataTypes[0];

            this.Data = new IncrementalLoadingList<IModel>(GetPinsAsync)
            {
                AfterAddItems = _ =>
                {
                    if (ExtendedGridView != null)
                    {
                        ExtendedGridView.Width = double.NaN;
                    }
                }
            };
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

        private string _SearchKey;
        public string SearchKey
        {
            get { return _SearchKey; }
            set { SetValue(ref _SearchKey, value); }
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

        private DelegateCommand _SearchCommand;
        public DelegateCommand SearchCommand
        {
            get
            {
                return _SearchCommand ?? (_SearchCommand = new DelegateCommand(
                async o =>
                {
                    try
                    {
                        var e = o as AutoSuggestBoxQuerySubmittedEventArgs;
                        if (string.IsNullOrEmpty(e?.QueryText))
                            return;

                        this.SearchKey = e.QueryText;
                        currentPage = 0;

                        await this.Data.ClearAndReload();
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
            if (string.IsNullOrWhiteSpace(this.SearchKey))
            {
                this.Data.NoMore();
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