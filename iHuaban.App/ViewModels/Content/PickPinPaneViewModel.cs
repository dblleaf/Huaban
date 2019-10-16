using iHuaban.App.Helpers;
using iHuaban.App.Models;
using iHuaban.App.Services;
using iHuaban.Core.Commands;
using iHuaban.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace iHuaban.App.ViewModels
{
    public class PickPinPaneViewModel : ViewModelBase
    {
        private IApiHttpHelper httpHelper;
        private Context Context;
        private IThemeService themeService;
        private IAccountService accountService;
        public PickPinPaneViewModel(IApiHttpHelper httpHelper,
            IThemeService themeService,
            IAccountService accountService,
            Context context)
        {
            this.httpHelper = httpHelper;
            this.themeService = themeService;
            this.accountService = accountService;
            this.Context = context;
            BoardList = new IncrementalLoadingList<Board>(GetMyBoards);
        }
        private IncrementalLoadingList<Board> _BoardList;
        public IncrementalLoadingList<Board> BoardList
        {
            get { return _BoardList; }
            set { SetValue(ref _BoardList, value); }
        }

        private Board _SelectedBoard;
        public Board SelectedBoard
        {
            get { return _SelectedBoard; }
            set { SetValue(ref _SelectedBoard, value); }
        }

        private Pin _Pin;
        public Pin Pin
        {
            get { return _Pin; }
            set { SetValue(ref _Pin, value); }
        }

        public ElementTheme GetRequestTheme()
        {
            return this.themeService.RequestTheme;
        }

        internal Popup Parent;
        private DelegateCommand _SelectBoardCommand;
        public DelegateCommand SelectBoardCommand
        {
            get
            {
                return _SelectBoardCommand ?? (_SelectBoardCommand = new DelegateCommand(
                async o =>
                {
                    try
                    {
                        if (Parent != null)
                        {
                            var dispatcher = Window.Current.Dispatcher;
                            await Task.Run(async () =>
                            {
                                var result = await accountService.PickPinAsync(Pin, this.SelectedBoard.board_id);
                                if (result.Pin.pin_id > 0)
                                {
                                    await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                    {
                                        this.Context.QuickBoard = this.SelectedBoard;
                                        Context.ShowMessage($"已采集到：{ this.SelectedBoard.title}");
                                        Parent.IsOpen = false;
                                        this.SelectedBoard = null;
                                    });
                                }
                            });
                        }

                    }
                    catch (Exception)
                    { }

                }, o => true));
            }
        }

        private DelegateCommand _HideCommand;
        public DelegateCommand HideCommand
        {
            get
            {
                return _HideCommand ?? (_HideCommand = new DelegateCommand(
                o =>
                {
                    try
                    {
                        if (Parent != null)
                        {
                            Parent.IsOpen = false;
                        }
                    }
                    catch (Exception)
                    { }

                }, o => true));
            }
        }

        private bool isLoadingBoards = false;
        private async Task<IEnumerable<Board>> GetMyBoards(uint startIndex, int page)
        {
            if (isLoadingBoards || this.Context.User == null)
            {
                return new List<Board>();
            }

            try
            {
                isLoadingBoards = true;
                string urlname = !string.IsNullOrEmpty(this.Context.User.urlname) ? this.Context.User.urlname : this.Context.User.user_id;

                var query = "?limit=20";
                var max = GetMaxKeyId();
                if (max > 0)
                {
                    query += $"&max={max}";
                }

                var collection = await httpHelper.GetAsync<BoardCollection>($"{urlname}/boards/{query}");
                var result = collection.Boards;

                if (result.Count() == 0)
                {
                    BoardList.NoMore();
                }
                else
                {
                    BoardList.HasMore();
                }

                return result;
            }
            catch { }
            finally
            {
                isLoadingBoards = false;
            }
            return null;
        }

        private long GetMaxKeyId()
        {
            if (BoardList?.Count > 0 && long.TryParse(BoardList[BoardList.Count - 1].KeyId, out long maxId))
            {
                return maxId;
            }
            else
            {
                return 0;
            }
        }

    }
}
