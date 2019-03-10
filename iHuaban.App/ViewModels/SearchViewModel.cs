using iHuaban.App.Models;
using iHuaban.Core.Commands;
using iHuaban.Core.Helpers;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using static System.Net.WebUtility;

namespace iHuaban.App.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {
        private IHttpHelper HttpHelper { get; set; }
        public SearchViewModel(IHttpHelper httpHelper)
        {
            this.HttpHelper = httpHelper;
            this.Data = new IncrementalLoadingList<IModel>(GetData);
            this.DataTypes = new ObservableCollection<DataType>()
            {
                new DataType("采集", Constants.ApiSearchPins, Loader<PinCollection ,Pin>),
                new DataType("画板", Constants.ApiSearchBoards, Loader<BoardCollection, Board>),
                new DataType("用户", Constants.ApiSearchUsers, Loader<UserCollection, User>),
            };

            this.DataType = DataTypes[0];
        }

        private IncrementalLoadingList<IModel> _Data;
        public IncrementalLoadingList<IModel> Data
        {
            get { return _Data; }
            set { SetValue(ref _Data, value); }
        }

        private DataType _DataType;
        public DataType DataType
        {
            get { return _DataType; }
            set { SetValue(ref _DataType, value); }
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

        public ObservableCollection<DataType> DataTypes { get; }

        

        private async Task<IEnumerable<IModel>> Loader<T, T2>(string url)
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

        private int page = 0;
        private async Task<IEnumerable<IModel>> GetData(uint startIndex, int page)
        {
            if (string.IsNullOrWhiteSpace(SearchKey))
            {
                return new List<IModel>();
            }

            if (string.IsNullOrWhiteSpace(SearchKey))
            {
                Data.NoMore();
                return null;
            }
            IsLoading = true;
            try
            {
                string url = $"{this.DataType.Url}?q={UrlEncode(this.SearchKey)}&page={++page}&per_page=20";
                var result = await this.DataType.DataLoaderAsync.Invoke(url);

                if (result.Count() > 0)
                    Data.HasMore();
                else
                    Data.NoMore();

                return result;
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
                        page = 0;
                        await this.Data.ClearAndReload();
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
                        page = 0;
                        await this.Data.ClearAndReload();
                    }
                    catch (Exception)
                    {

                    }

                }, o => true));
            }
        }
    }
}
