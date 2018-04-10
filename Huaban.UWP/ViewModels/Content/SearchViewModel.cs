using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Huaban.UWP.ViewModels
{
    using Base;
    using Commands;
    using Huaban.UWP.Services;
    using Models;
    using Unity.Attributes;

    public class SearchViewModel : HBViewModel
    {

        public SearchViewModel(Context context)
            : base(context)
        {
            LeftHeaderVisibility = Windows.UI.Xaml.Visibility.Collapsed;
            PinListViewModel = new PinListViewModel(context, GetData);
            KeyWord = "";
            Title = "搜索";
        }

        #region Properties
        [Dependency]
        public PinService PinService { get; set; }
        public PinListViewModel PinListViewModel { set; get; }

        private string KeyWord { set; get; }

        #endregion

        #region Commands

        private DelegateCommand _QueryCommand;
        public DelegateCommand QueryCommand
        {
            get
            {
                return _QueryCommand ?? (_QueryCommand = new DelegateCommand(
                async o =>
                {
                    try
                    {
                        var e = o as AutoSuggestBoxQuerySubmittedEventArgs;
                        if (string.IsNullOrEmpty(e?.QueryText))
                            return;
                        KeyWord = e.QueryText;

                        await PinListViewModel.ClearAndReload();
                    }
                    catch (Exception ex)
                    {

                    }

                }, o => true));
            }
        }

        #endregion

        #region Methods

        private async Task<IEnumerable<Pin>> GetData(uint startIndex, int page)
        {
            PinListViewModel.PinList.NoMore();

            if (string.IsNullOrEmpty(KeyWord))
                return new List<Pin>();

            IsLoading = true;
            try
            {

                var list = await PinService.Search(KeyWord, page);
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
        public override Size ArrangeOverride(Size finalSize)
        {
            return finalSize;
        }
        #endregion
    }
}
