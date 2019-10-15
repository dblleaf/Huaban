using iHuaban.App.Models;
using iHuaban.Core.Commands;
using iHuaban.Core.Models;
using System;

namespace iHuaban.App.ViewModels
{
    public class BoardSelectorViewModel : ViewModelBase
    {
        private IncrementalLoadingList<Board> _BoardList;
        public IncrementalLoadingList<Board> BoardList
        {
            get { return _BoardList; }
            set { SetValue(ref _BoardList, value); }
        }

        public Action<Board> AfterSelectBoard { get; set; }

        private DelegateCommand _SelectBoardCommand;
        public DelegateCommand SelectBoardCommand
        {
            get
            {
                return _SelectBoardCommand ?? (_SelectBoardCommand = new DelegateCommand(
                o =>
                {
                    try
                    {

                    }
                    catch (Exception)
                    {

                    }

                }, o => true));
            }
        }

    }
}
